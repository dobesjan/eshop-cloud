using Filestore.Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IO;

namespace Filestore.Api.Controllers
{
	[ApiController]
	[Route("/")]
	public class FileStoreController : ControllerBase
	{
		private readonly ILogger<FileStoreController> _logger;
		private readonly FilestoreOptions _options;

		public FileStoreController(ILogger<FileStoreController> logger, IOptions<FilestoreOptions> options)
		{
			_logger = logger;
			_options = options.Value;
		}

		[HttpGet(Name = "Image")]
		public IActionResult GetImage(string fileName)
		{
			var path = Path.Combine(_options.ImagesPath, fileName);
			try
			{
				var fileBytes = LoadFile(path);
				return File(fileBytes, "image/jpeg");
			}
			catch (FileNotFoundException ex)
			{
				_logger.LogError(ex.ToString());
				return NotFound();
			}
		}

		[HttpGet(Name = "Invoice")]
		public IActionResult GetInvoice(string fileName)
		{
			var path = Path.Combine(_options.InvoicesPath, fileName);
			try
			{
				var fileBytes = LoadFile(path);
				return File(fileBytes, "application/pdf");
			}
			catch (FileNotFoundException ex)
			{
				_logger.LogError(ex.ToString());
				return NotFound();
			}
		}

		private byte[]? LoadFile(string path)
		{
			if (!System.IO.File.Exists(path))
			{
				throw new FileNotFoundException(path);
			}

			return System.IO.File.ReadAllBytes(path);
		}
	}
}
