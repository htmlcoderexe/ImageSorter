using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSorter
{
    public class ImageSorterSingleton
    {
        /// <summary>
        /// If a file move fails, this will contain the reason if possible.
        /// </summary>
        public enum WriteErrorReason
        {
            ///<summary>No error.</summary>
            None,
            ///<summary>Generic I/O error.</summary>
            IOError,
            ///<summary>The user doesn't have write access to the target location.</summary>
            AccessDenied,
            ///<summary>The target path is too long for this filesystem.</summary>
            PathTooLong,
            /// <summary>The target location is gone.</summary>
            TargetMissing,
            ///<summary>An unknown error prevented the operation.</summary>
            Other

        }

        /// <summary>
        /// Represents a result from processing the current file.
        /// </summary>
        public enum FileProcessResult
        {
            // just in some weird corner case of a variable
            // typed to be this enum and initialising to 0
            // or a 0 being cast to this enum in any other fashion,
            // have 0 mean a (probably unused yet) error condition
            // to not assume success state
            /// <summary>The operation failed due to an unknown error.</summary>
            UnknownError,
            /// <summary>The file was successfully moved to a new location.</summary>
            Success,
            /// <summary>The file was not moved because the programw was unable to write the target file.</summary>
            WriteError,
            /// <summary>The file to be moved is no longer accessible.</summary>
            FileIsMissing,
            /// <summary>The requested key was not assigned to a folder.</summary>
            NoKeyBinding,
            /// <summary>The target location already contains a file with this name.</summary>
            TargetExists
        }

        /// <summary>
        /// Represents the outcome of opening a folder.
        /// </summary>
        public enum OpenFolderResult
        {
            ///<summary>The directory was not found.</summary>
            NotFound,
            ///<summary>The directory was opened successfully.</summary>
            Success,
            ///<summary>The directory was inaccessible.</summary>
            AccessDenied,
            ///<summary>The directory path was too long.</summary>
            PathTooLong,
            ///<summary>The directory was opened successfully, but one or more keybind loads failed.</summary>
            KeybindFailed,
            ///<summary>The directory was opened succesfully, but no keybinds were able to load - program won't be able to operate.</summary>
            AllKeybindsFailed,
            ///<summary>The directory is readonly.</summary>
            ReadOnly,
            ///<summary>There were no images in the directory.</summary>
            NoImages,
            ///<summary>Unknown cause of failure.</summary>
            Other
        }

        private static ImageSorterSingleton _instance;
        /// <summary>
        /// Gets the ImageSorter instance.
        /// </summary>
        /// <returns></returns>
        public static ImageSorterSingleton GetInstance()
        {
            if(_instance == null)
            {
                _instance = new ImageSorterSingleton();
            }
            return _instance;
        }

        /// <summary>
        /// Constructs the one and only insance os this amazing, magnificent class.
        /// </summary>
        private ImageSorterSingleton()
        {
            // initialise some state stuff
            SubFolders = new Dictionary<char, string>();
            UndoStack = new Stack<Tuple<string, string>>();
            // not the file list, it becomes null whenever not in sorting mode
        }

        // constants

        // the program will try to look for this file for its settings
        const string ConfigFileName = "ImageSorter.cfg";
        // This character will separate keys and assigned folders 
        // chosen because it is unlikely to occur as a keypress,
        // looks like ":=" asignment operator and is actually
        // a mathematical symbol (one of!) for assigning a variable.
        const char Separator = '≔';
        // hardcoding the image extensions here for now.
        public static readonly string[] extensions = { ".png", ".jpg", ".gif", ".jpeg", ".webp" };

        // internal state

        
        // queue to progress through the folder
        LinkedList<string> Todo;
        // this is the dictionary that will hold all the keybindings
        Dictionary<char, string> SubFolders;
        // this will hold the Undo stack
        Stack<Tuple<string, string>> UndoStack;
        // this will hold the backup Undo stack
        Stack<Tuple<string, string>> UndoBackup;

        // public state

        public string ProgramRootDir { get; set; }

        /// <summary>
        /// Determines whether the ImageSorter has an active list of files to process.
        /// </summary>
        public bool IsActive { get => Todo != null; }

        /// <summary>
        /// Checks if there are any ongoing operations at the moment.
        /// </summary>
        public int PendingOps { get; private set; }

        /// <summary>
        /// Current directory worked on.
        /// </summary>
        public DirectoryInfo CurrentDir { get; private set; }

        /// <summary>
        /// Current file worked on.
        /// </summary>
        public string CurrentFile { get; private set; }

        /// <summary>
        /// Filename of the current file.
        /// </summary>
        public string CurrentFileName { get => Path.GetFileName(CurrentFile); }
        

        /// <summary>
        /// Amount of files already processed.
        /// </summary>
        public int FilesDone
        {
            get
            {
                if (UndoStack == null)
                    return 0;
                return UndoStack.Count();
            }
        }


        /// <summary>
        /// Amount of files awaiting processing.
        /// </summary>
        public int FilesRemaining
        {
            get
            {
                if (Todo == null)
                    return 0;
                // files in the queue
                return Todo.Count;
            }
        }

        /// <summary>
        /// Total amount of files.
        /// </summary>
        public int FilesTotal
        {
            get
            {
                // files already done + the current file + the queue
                return FilesDone + 1 + FilesRemaining;
            }
        }

        /// <summary>
        /// Defines the specifics of the last file write error.
        /// </summary>
        public WriteErrorReason LastWriteError { get; private set; }

        /// <summary>
        /// Full path of the last move operation target, regardless of success.
        /// </summary>
        public string LastUsedTargetFolder { get; private set; }

        /// <summary>
        /// Filename to use for the current file to be processed.
        /// </summary>
        public string NewFileName { get; set; } = "";

        /// <summary>
        /// Original or changed filename to be used on move.
        /// </summary>
        public string CurrentTargetFileName { get => NewFileName == "" ? Path.GetFileName(CurrentFile) : NewFileName; }

        /// <summary>
        /// Gets a copy of the keybindings.
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<char, string>> GetBindings()
        {
            List<KeyValuePair<char, string>> result = new List<KeyValuePair<char, string>>();
            if(SubFolders == null)
                return result;
            foreach(KeyValuePair<char, string> kvp in SubFolders)
                result.Add(kvp);
            return result;
        }

        // helper methods

        /// <summary>
        /// Check if filename contains any invalid characters.
        /// </summary>
        /// <param name="s">Filename to check</param>
        /// <returns></returns>
        public static bool IsNameInvalid(string s)
        {
            if (s.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
                return true;
            return false;
        }

        /// <summary>
        /// Tries to find a filename that would not collide with an existing one.
        /// </summary>
        /// <param name="fileName">The colliding file name.</param>
        /// <param name="folder">Target folder</param>
        /// <returns></returns>
        public static string GetFreeFileName(string fileName, string folder)
        {
            string ext = Path.GetExtension(fileName);
            string name = Path.GetFileNameWithoutExtension(fileName);

            int counter = 2;
            string candidatename = name + "_" + counter.ToString() + ext;

            while (File.Exists(Path.Combine(folder, candidatename)))
            {
                counter++;
                candidatename = name + "_" + counter.ToString() + ext;
            }

            return candidatename;
        }
        /// <summary>
        /// Tries to find a filename that would not collide with an existing one, in a specific target folder.
        /// </summary>
        /// <param name="key">The keypress identifying the target folder to search.</param>
        /// <returns></returns>
        public string GetFreeFileName()
        {
            return GetFreeFileName(CurrentTargetFileName, LastUsedTargetFolder);
        }

        /// <summary>
        /// Try parsing a configuration line.
        /// </summary>
        /// <param name="line">Line to be parsed.</param>
        /// <returns>true on success, false on faliure</returns>
        public static KeyValuePair<char, string>? ParseConfigLine(string line)
        {
            //bail early if empty
            if(line == "" || line == null)
                return null;
            //split the line into the keychar and the folder name
            string[] parts = line.Split(new char[] { Separator }, 2);
            //sanity check, if there are not two parts, fail
            if(parts.Length != 2)
                return null;
            if(parts[0].Length > 1)
                return null;
            //the first(and only) char of the first piece is the keychar
            char keychar = parts[0][0];
            //the second piece is the foldername
            string folder = parts[1];
            //check if foldername is valid for usage
            if(IsNameInvalid(folder))
                return null;
            return new KeyValuePair<char, string>(keychar, folder);
        }

        /// <summary>
        /// Try parsing a configuration file.
        /// </summary>
        /// <param name="lines">Array of configuration lines</param>
        /// <returns>List of binding KVPs or empty list on failure</returns>
        public static List<KeyValuePair<char, string>> ParseConfigFile(string[] lines)
        {
            List<KeyValuePair<char, string>> configs = new List<KeyValuePair<char, string>>();
            //go over each line
            for(int i = 0; i < lines.Length; i++)
            {
                //try getting a valid config line
                KeyValuePair<char, string>? kvp = ParseConfigLine(lines[i]);
                //if line is valid, add to the config list
                if(kvp.HasValue)
                {
                    configs.Add(kvp.Value);
                }
                else
                {
                    //last line is allowed to be invalid (most likely empty)
                    //anywhere else, return empty list/fail
                    if(i != lines.Length - 1)
                    {
                        return new List<KeyValuePair<char, string>>();
                    }
                }
            }
            //if not failed yet, return what is collected so far
            return configs;
        }

        /// <summary>
        /// Try loading a configuration file from a folder
        /// </summary>
        /// <param name="folderpath">Folder containing the file</param>
        /// <returns>List of binding KVPs or null on fail</returns>
        public static List<KeyValuePair<char, string>> LoadConfigFile(string folderpath)
        {
            List<KeyValuePair<char, string>> configs = new List<KeyValuePair<char, string>>();
            //get the proper filename and try loading
            string filepath = Path.Combine(folderpath, ConfigFileName);
            //if file's missing, fail
            if(!File.Exists(filepath))
                return null;
            string[] filecontents;
            try
            {
                //try reading the file as lines and parsing those
                filecontents = File.ReadAllLines(filepath);
                configs = ParseConfigFile(filecontents);
                //if non-empty list is returned, return it
                if(configs.Count > 0)
                    return configs;
                //else fail
                return null;
            }
            //couldn't load, don't care too much why right now, fail
            catch(Exception ex)
            {
                return null;
            }
        }

        bool SaveConfigFile(string folderpath)
        {
            //if there are no bindings, don't bother and fail
            if(SubFolders.Count < 1)
                return false;
            string[] lines = new string[SubFolders.Count];
            //squish the dictionary
            KeyValuePair<char, string>[] bindings = SubFolders.ToArray();
            //go slot per slot
            for(int i = 0; i < bindings.Length; i++)
            {
                //each line is the keychar, then ≔, then the subfolder
                lines[i] = bindings[i].Key.ToString() + Separator.ToString() + bindings[i].Value;
            }
            //attempt to write the config to disk
            try
            {
                File.WriteAllLines(Path.Combine(folderpath, ConfigFileName), lines);
            }
            //if write fails, so does this function
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool SaveCurrentConfig()
        {
            return SaveConfigFile(CurrentDir.FullName);
        }

        List<KeyValuePair<char, string>> TryLoadConfig(string path)
        {
             // try loading a configuration file from the target folder
            List<KeyValuePair<char, string>> configs = LoadConfigFile(path);

            // if successful, load all keybinds from it
            if(configs != null)
            {
                return configs;
            }
            // else try default config (should be in the application's own folder)
            // for now there's no way to adjust it except for copying one created for a specific folder
 
            configs = LoadConfigFile(Path.GetDirectoryName(ProgramRootDir));
            // if successful, load all keybinds from it
            if(configs != null)
            {
                return configs;
            }
            // if that fails, load this default hardcoded preset
            else
            {
                return new List<KeyValuePair<char, string>> { new KeyValuePair<char, string>(' ', "nsfw") };
            }
            
        }



        /// <summary>
        /// Loads a list of all filenames with image-like extensions from a given folder.
        /// </summary>
        /// <param name="path">Folder path to search</param>
        /// <returns>List of filenames</returns>
        public static List<string> GetImageFiles(string path)
        {
            List<string> result = new List<string>();

            foreach (string ext in extensions)
            {
                result.AddRange(Directory.EnumerateFiles(path, "*" + ext, SearchOption.TopDirectoryOnly));
            }
            return result;
        }


        public KeyValuePair<char,string>? LoadKeyBind(string path, char key, string target)
        {
            // create the corresponding folder
            try
            {
                Directory.CreateDirectory(Path.Combine(path, target));
            }
            // usually this only fails if there is a file named like the target exactly, in which case, try a random name
            catch(Exception)
            {

                string newrandomstring = Path.GetRandomFileName();
                target = target + "_" + newrandomstring;
                try
                {
                    Directory.CreateDirectory(Path.Combine(path, target));
                }
                // either something else went wrong, or we're astronomically unlucky - return a fail
                catch(Exception)
                {
                    return null;
                }
            }
            // return the binding actually created
            return new KeyValuePair<char, string>(key, target);
        }



        /// <summary>
        /// Load a folder for processing.
        /// </summary>
        /// <param name="path">Absolute path to the folder.</param>
        public OpenFolderResult OpenFolder(string path)
        {
            // fail early if the folder doesn't even exist
            if(!Directory.Exists(path))
                return OpenFolderResult.NotFound;

            // Set current working dir
            DirectoryInfo workingDir = new DirectoryInfo(path);

            //check if dir is readonly
            if(workingDir.Attributes.HasFlag(FileAttributes.ReadOnly))
                return OpenFolderResult.ReadOnly;

            // fetch a list of all images
            List<string> FileNames = GetImageFiles(path);

            // if there were no images, return
            if(FileNames.Count < 1)
                return OpenFolderResult.NoImages;

            // create and populate a queue to process one item at a time
            LinkedList<string> workingQueue = new LinkedList<string>();
            foreach(string filepath in FileNames)
            {
                workingQueue.AddLast(filepath);
            }

            // try loading keybinds from the chosen folder
            List<KeyValuePair<char, string>> configs = TryLoadConfig(path);
            // create and populate a dictionary for attempted loads
            Dictionary<char, string> tempKeyBinds = new Dictionary<char, string>();
            foreach(KeyValuePair<char, string> kvp in configs)
            {
                KeyValuePair<char, string>? bind = LoadKeyBind(path, kvp.Key, kvp.Value);
                if(bind.HasValue)
                    tempKeyBinds.Add(bind.Value.Key, bind.Value.Value);
            }
            // if every load failed, target folder was unusable
            if(tempKeyBinds.Count < 1)
                return OpenFolderResult.AllKeybindsFailed;
            // otherwise, still possible to use - setup internal state
            SubFolders = tempKeyBinds;
            UndoStack.Clear();
            Todo = workingQueue;
            CurrentDir = workingDir;
            NewFileName = "";
            // if there were any fails at all, note this
            if(tempKeyBinds.Count < configs.Count)
                return OpenFolderResult.KeybindFailed;
            // if no fails, return success
            return OpenFolderResult.Success;

        }

        public OpenFolderResult ReloadFolder()
        {
            OpenFolderResult result;
            BackupUndo();
            result = OpenFolder(CurrentDir.FullName);
            RestoreUndo();
            return result;
        }

        /// <summary>
        /// Attempts to process the current file being worked on.
        /// </summary>
        /// <param name="Key">The character shortcut entered by user.</param>
        /// <returns>The result of processing.</returns>
        public FileProcessResult ProcessCurrentFile(char Key)
        {
            // check if a valid keybinding was passed
            if(!SubFolders.ContainsKey(Key))
            {
                return FileProcessResult.NoKeyBinding;
            }

            // check if the file went missing while the user was staring at the buttons
            // sometimes that happens
            if(!File.Exists(CurrentFile))
            {
                return FileProcessResult.FileIsMissing;
            }

            // prepare target filepath
            string targetfilename = CurrentTargetFileName;
            string targetfolder = SubFolders[Key];
            string targetpath = Path.Combine(CurrentDir.FullName, targetfolder, targetfilename);

            // keep track of the last folder
            LastUsedTargetFolder = Path.Combine(CurrentDir.FullName, targetfolder);

            // check if the filename is available on the target
            if(File.Exists(targetpath))
            {
                return FileProcessResult.TargetExists;
            }

            // so far so good, try moving the file
            try
            {
                File.Move(CurrentFile, targetpath);
            }
            catch (PathTooLongException)
            {
                LastWriteError = WriteErrorReason.PathTooLong;
                return FileProcessResult.WriteError;
            }
            catch (UnauthorizedAccessException)
            {
                LastWriteError = WriteErrorReason.AccessDenied;
                return FileProcessResult.WriteError;
            }
            catch (DirectoryNotFoundException)
            {
                LastWriteError = WriteErrorReason.TargetMissing;
                return FileProcessResult.WriteError;
            }
            catch (IOException)
            {
                LastWriteError = WriteErrorReason.IOError;
                return FileProcessResult.WriteError;

            }
            catch(Exception)
            {
                LastWriteError = WriteErrorReason.Other;
                return FileProcessResult.WriteError;
            }

            LastWriteError = WriteErrorReason.None;

            // file moved successfully!

            // write the Undo entry
            UndoStack.Push(new Tuple<string,string>(targetpath, CurrentFile));

            return FileProcessResult.Success;

        }

        /// <summary>
        /// Moves onto the next file for processing.
        /// </summary>
        /// <returns>False if there is no next file, true otherwise.</returns>
        public bool Advance()
        {
            // exit if there's no queue
            if (Todo == null)
            {
                return false;
            }
            // if the queue is empty, the folder is complete
            // reset the queue
            if (Todo.Count < 1)
            {
                Todo = null;
                return false;
            }
            // get next item and set path
            string FilePath = Todo.First();
            Todo.RemoveFirst();
            CurrentFile = FilePath;
            // reset rename function
            NewFileName = "";

            return true;
        }

        public bool Undo()
        {
            // do nothing if there are no undos available, return success
            if(UndoStack == null || UndoStack.Count == 0)
                return true;
            // get the most recent operation
            Tuple<string, string> UndoEntry = UndoStack.Pop();
            // attempt to reverse the move
            try
            {
                File.Move(UndoEntry.Item2, UndoEntry.Item1);
            }
            catch(Exception)
            {
                // no point in doing anything else, return fail
                return false;
            }
            // put the current file back into the queue
            Todo.AddFirst(CurrentFile);
            // put the recently undone file back into the queue and advance to it
            Todo.AddFirst(UndoEntry.Item1);
            return Advance();
        }
        /// <summary>
        /// Temporarily holds the Undo history (in reverse order)
        /// </summary>
        void BackupUndo()
        {
            // hold my Undo history...
            UndoBackup = new Stack<Tuple<string, string>>();
            // shove it in there
            while(UndoStack.Count > 0)
            {
                UndoBackup.Push(UndoStack.Pop());
            }
        }

        void RestoreUndo()
        {
            UndoStack.Clear();
            // the load clears undo history, good thing we backed that up
            while(UndoBackup.Count > 0)
            {
                UndoStack.Push(UndoBackup.Pop());
            }
        }

    }
}
