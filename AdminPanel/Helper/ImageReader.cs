namespace AdminPanel.Helper
{
	public class ImageReader
	{
		public static IFormFile ReadImage(string path)
		{
			var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);

			FileStream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);

			IFormFile formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
			{
				Headers = new HeaderDictionary(),
				ContentType = "image/*"
			};
			return formFile;
		}
	}
}
