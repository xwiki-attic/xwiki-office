using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace XWriter
{
    /// <summary>
    /// Offers functionality for storing the repository settings to Isolated Storage.
    /// </summary>
    public class Repositories
    {
        //Isolated storage file name;
        static string filename = "XWordRepositoriesSettings";

        /// <summary>
        /// Writes the repository settings to Isolated Storage.
        /// </summary>
        /// <param name="settings">The settings to be saved.</param>
        /// <returns>True if the operation succeded. False if the operation failed.</returns>
        public static bool WriteRepositorySettings(RepositorySettings settings)
        {
            IsolatedStorageFile isFile = IsolatedStorageFile.GetUserStoreForAssembly();
            IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filename, FileMode.Create, isFile);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, settings);
            stream.Close();
            return true;
        }

        /// <summary>
        /// Gets the repository settings from Isolated Storage.
        /// </summary>
        /// <returns>A instance containing the repository settings.</returns>
        public static RepositorySettings GetRepositorySettings()
        {
            try
            {
                RepositorySettings settings = new RepositorySettings();
                IsolatedStorageFile isFile = IsolatedStorageFile.GetUserStoreForAssembly();
                IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filename, FileMode.Open, isFile);
                BinaryFormatter formatter = new BinaryFormatter();
                settings = (RepositorySettings)formatter.Deserialize(stream);
                return settings;
            }
            catch (InvalidCastException ce)
            {
                Log.ExceptionSummary(ce);
                return null;
            }
            catch (Exception ex)
            {
                Log.ExceptionSummary(ex);
                return null;
            }
        }

        /// <summary>
        /// Specifies if the application has the repository settings saved in Isolated Storage.
        /// </summary>
        /// <returns>
        /// True is the repository configuration file exists in Isolated Storage.
        /// False if the file does not exist.
        /// </returns>
        public static bool HasRepositorySettings()
        {
            IsolatedStorageFile isFile = IsolatedStorageFile.GetUserStoreForAssembly();
            return (isFile.GetFileNames(filename).Length > 0);
        }
    }
}
