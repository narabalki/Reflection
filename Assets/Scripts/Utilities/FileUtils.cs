using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using System;
using Newtonsoft.Json.Linq;

public class FileUtils  {

	public static void  DeleteDirectory (string dirPath, bool recursive){
		if (Directory.Exists (dirPath))
			Directory.Delete (dirPath, recursive);
	}

	public static void CopyFile (string fromPath, string toPath, bool overWrite)
	{
		if (File.Exists (fromPath)) {
			string dirPath = Path.GetDirectoryName (toPath);
			Directory.CreateDirectory (dirPath);
			File.Copy (fromPath, toPath, overWrite);
		}else
			Debug.LogError ("File Not Exist at:" + fromPath);
	}

	public static void CopyDirectory (string fromDir, string toDir, bool overWrite = false)
	{
		if (!Directory.Exists (toDir)) {
			Directory.CreateDirectory (toDir);
		}

		DirectoryInfo dirInfo = new DirectoryInfo (fromDir);
		FileInfo[] files = dirInfo.GetFiles ();
		foreach (FileInfo tempfile in files) {
			string fromPath = tempfile.FullName;
			string toPath = Path.Combine (toDir, tempfile.Name);
			CopyFile (fromPath, toPath, overWrite);
		}

		DirectoryInfo[] directories = dirInfo.GetDirectories ();
		foreach (DirectoryInfo tempdir in directories) {
			string sourceDirPath = Path.Combine (fromDir, tempdir.Name);
			string destDirPath = Path.Combine (toDir, tempdir.Name);
			if(overWrite)
				DeleteDirectory (destDirPath,true);
			CopyDirectory (sourceDirPath, destDirPath);
		}
	}

	public static void MoveDirectory (string strSource, string strDestination)
	{
		if (!Directory.Exists (strDestination)) {
			Directory.CreateDirectory (strDestination);
		}

		DirectoryInfo dirInfo = new DirectoryInfo (strSource);
		FileInfo[] files = dirInfo.GetFiles ();
		foreach (FileInfo tempfile in files) {
			string dest = Path.Combine (strDestination, tempfile.Name);
			if (File.Exists(dest))
				File.Delete(dest);
			tempfile.MoveTo (dest);
		}

		DirectoryInfo[] directories = dirInfo.GetDirectories ();
		foreach (DirectoryInfo tempdir in directories) {
			MoveDirectory (Path.Combine (strSource, tempdir.Name), Path.Combine (strDestination, tempdir.Name));
		}
		if (!Directory.Exists (strSource)) 
			Directory.Delete (strSource,true);
	}

	public static string checkMD5(string filename)
	{
		using (var md5 = MD5.Create())
		{
			using (var stream = File.OpenRead(filename))
			{
				return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-","").ToLower();
			}
		}
	}

	public static string ReadContentFromStreamingAssets(string fileName){
		#if UNITY_EDITOR || UNITY_IOS
		return File.ReadAllText(Application.streamingAssetsPath +"/"+ fileName);
		#elif UNITY_ANDROID 
		return AndroidUtils.readFromAssetsFile(fileName);
		#endif
	}

	public static void CopyDirFromStreamingAssetsIfNotExist(string dirPath, string destDirPath){
		if (!Directory.Exists (destDirPath)) {    
			CopyDirFromStreamingAssets (dirPath, destDirPath);		
		}
	}
	public static void CopyDirFromStreamingAssets(string dirPath, string destDirPath){
		#if UNITY_EDITOR || UNITY_IOS
		CopyDirectory (Application.streamingAssetsPath + "/"+dirPath, destDirPath);	
		#elif UNITY_ANDROID 
		AndroidUtils.copyFSResource(dirPath, destDirPath);
		#endif
	}

	public static void CopyFileFromStramingAssets(string filename, string destPath, bool encode, bool overwrite=false)
	{
		if (!overwrite && File.Exists (destPath))
			return;

		#if UNITY_EDITOR || UNITY_IOS
		File.Copy (Application.streamingAssetsPath + "/" + filename, destPath, overwrite);
		#elif UNITY_ANDROID 
		string plainText = AndroidUtils.readFromAssetsFile(filename);
		if(encode)
		EncryptIntoFile(destPath,plainText);
		else
		File.WriteAllText(destPath,plainText);
		#endif
	}

	public string ReadFileFromStreamingAssets(string filename){
		string plainText = "";
		#if UNITY_EDITOR || UNITY_IOS
		string filePath = Application.streamingAssetsPath + "/" + filename;
		plainText = File.ReadAllText(filePath);
		#elif UNITY_ANDROID 
		plainText = AndroidUtils.readFromAssetsFile(filename);
		#endif
		return plainText;
	}

	public static string ReadContent(string filePath,bool isEncrypted){
		#if UNITY_EDITOR || UNITY_IOS
		return File.ReadAllText(filePath);
		#elif UNITY_ANDROID 
		if(isEncrypted)
		return GetDecryptedContent(filePath);
		else
		return File.ReadAllText(filePath);
		#endif
	}

	public static void WriteContent(string plainText, string filePath, bool encode){
		#if UNITY_EDITOR || UNITY_IOS
		File.WriteAllText(filePath,plainText);
		#elif UNITY_ANDROID 
		if(encode)
		EncryptIntoFile(filePath,plainText);
		else
		File.WriteAllText(filePath,plainText);
		#endif
	}

	#if false
	static string GetDecryptedContent(string filePath){
		string cipherText = File.ReadAllText(filePath);
		string decryptedData = Encrypt.DecryptString(cipherText, Encrypt.PASSWORD);
		return decryptedData;
	}

	static void EncryptIntoFile(string filePath, string plainText){
		string ciperText = Encrypt.EncryptString(plainText,Encrypt.PASSWORD);
		File.WriteAllText(filePath,ciperText);
	}

	public static bool IsEncryptedJsonFile(string filePath){
		string cipherText = File.ReadAllText(filePath);
		string decryptedData = Encrypt.DecryptString(cipherText, Encrypt.PASSWORD);
		try
		{
			JToken token = JObject.Parse(decryptedData);
			return true;
		}
		catch(Exception ex)
		{
			return false;
		}
	}
	#endif
}
