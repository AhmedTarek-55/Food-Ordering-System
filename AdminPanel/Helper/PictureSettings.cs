namespace AdminPanel.Helper
{
	public class PictureSettings
	{
		public static string UploadFile (IFormFile file, string folderName)
		{
			// 1- Get the path of the folder passed
			var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folderName);

			// 2- Get a unique name for the file
			var FileName = $"{Guid.NewGuid()}-{Path.GetFileName(file.FileName)}";

			// 3- Get the file path
			var FilePath = Path.Combine(FolderPath, FileName);

			// 4- Store the file on the server
			using var FileStream = new FileStream(FilePath, FileMode.Create);
			file.CopyTo(FileStream);

			// 5- Return file name
			return Path.Combine("images/products", FileName);
		}

		public static void DeleteFile(string filepath) 
		{
			var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filepath);

			if (File.Exists(FilePath))
				File.Delete(FilePath);
		}
	}
}
