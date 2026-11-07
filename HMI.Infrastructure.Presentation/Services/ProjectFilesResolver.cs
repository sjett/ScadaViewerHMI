// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using System.IO;

namespace HMI.Infrastructure.Presentation.Services
{
	public static class ProjectFilesResolver
	{
		public static string ResolveDeploymentFolder(IProjectViewModel project, MediaType genericMedia)
		{
			switch (genericMedia)
			{
			case MediaType.Image:
				return Path.Combine(project.AppProjectFolder, "Images");
			case MediaType.Xaml:
				return Path.Combine(project.AppProjectFolder, "Xamls");
			case MediaType.Audio:
				return Path.Combine(project.AppProjectFolder, "Media");
			default:
				throw new NotSupportedException();
			}
		}
		public static string Resolve(string fileName, MediaType mediaType)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return fileName;
			}
			return ResolveProjectRelativePath(fileName, mediaType);
		}
        private static string ResolveProjectRelativePath(string fileName, MediaType mediaType)
        {
			
            fileName = fileName.TrimStart(Path.DirectorySeparatorChar);
            switch (mediaType)
            {
                case MediaType.Image:
                    var segments = fileName.TrimStart('/', '\\').Split(new[] { '/', '\\' });
                    return "pack://siteoforigin:,,,/Images/" +
                        string.Join("/", segments.Select(Uri.EscapeDataString));
                case MediaType.Audio:
                    return "pack://siteoforigin:,,,/Media/" +
                        string.Join("/", fileName.TrimStart('/', '\\').Split(new[] { '/', '\\' }).Select(Uri.EscapeDataString));
            }
            throw new NotSupportedException();
        }
    }
}
