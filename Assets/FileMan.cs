using UnityEngine;
using System.IO;

public class FileMan : MonoBehaviour {




    public static bool CreateIfNotExisting(string full_system_path)
    { 

        if (File.Exists(full_system_path))
        {
            return false;
        }
    

        StreamWriter write_stream = File.CreateText(full_system_path);
        write_stream.Close();

        return true;
    }



	public static void Add2File(string line, string file_path)
	{

        file_path = Application.persistentDataPath + file_path;
        CreateIfNotExisting(file_path);

        if (File.Exists(file_path))
		{
			StreamWriter write_stream = File.AppendText(file_path);
			write_stream.WriteLine(line);
			write_stream.Close();
		}
		else {
			Debug.Log("File does not exist");
		}
	}




	public static void RewriteFile(string content, string file_path)
	{

       // Debug.Log("file_path arg: " + file_path);
        //file_path = Application.persistentDataPath + "/" +file_path;
        //Debug.Log("after adding persistentPath arg: " + file_path);
        CreateIfNotExisting(file_path);

        if (File.Exists(file_path))
		{
			File.WriteAllText(file_path, content);
		}

	}


	public static string ReadFileContent(string file_path)
	{

        string content =string.Empty;

		if(File.Exists(file_path))
		{
			StreamReader stream_read = File.OpenText(file_path);

			string line = stream_read.ReadLine();
			while(line != null)
			{
				content = content + line;
				line = stream_read.ReadLine();
			}

			stream_read.Close();
			return content;

		}
		else
		{
			Debug.Log("Could not open the file");
			return string.Empty;
		}
       

	}


}
