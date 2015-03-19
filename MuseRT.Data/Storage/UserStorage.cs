using PCLStorage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MuseRT.Data
{
    public class UserStorage
    {
        /// <summary>
        /// Reads the contents of a file as a string
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<string> ReadTextFromFile(string fileName)
        {
            try
            {
                var folder = FileSystem.Current.LocalStorage;
                var file = await folder.GetFileAsync(fileName);
                if (file != null)
                {
                    return await file.ReadAllTextAsync();
                }
            }
            catch (FileNotFoundException)
            {
            }
            catch (Exception ex)
            {
                Logger.WriteError("UserStorage.ReadTextFromFile", ex);
            }
            return String.Empty;
        }

        /// <summary>
        /// Writes text to given file, overwriting any existing data
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task WriteText(string fileName, string content)
        {
            try
            {
                var folder = FileSystem.Current.LocalStorage;
                var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await file.WriteAllTextAsync(content);
            }
            catch (Exception ex)
            {
                Logger.WriteError("UserStorage.WriteText", ex);
            }
        }

        /// <summary>
        /// Deletes the file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task DeleteFileIfExists(string fileName)
        {
            try
            {
                var folder = FileSystem.Current.LocalStorage;
                var file = await folder.GetFileAsync(fileName);
                if (file != null)
                {
                    await file.DeleteAsync();
                }
            }
            catch (FileNotFoundException)
            {
            }
            catch (Exception ex)
            {
                Logger.WriteError("UserStorage.DeleteFileIfExists", ex);
            }
        }

        /// <summary>
        /// Appends text to given file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static async Task AppendLineToFile(string fileName, string line)
        {
            try
            {
                var folder = FileSystem.Current.LocalStorage;
                var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                string old = await file.ReadAllTextAsync();
                await file.WriteAllTextAsync(old + Environment.NewLine + line);
            }
            catch { /* Avoid any exception at this point. */ }
        }
    }
}
