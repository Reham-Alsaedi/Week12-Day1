using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace ImageProcessingApi.Services
{
    public interface IImageProcessingService
    {
        byte[] ApplyFilter(byte[] imageData, string filter);
    }

    public class ImageProcessingService : IImageProcessingService
    {
        // Defines a method that accepts raw image bytes and a filter name, and returns the processed image as bytes.
        public byte[] ApplyFilter(byte[] imageData, string filter)
        {
            // Loads the image from the input byte array into an Image object.
            using var image = Image.Load(imageData);

            // If the requested filter is "grayscale" (case-insensitive), apply grayscale transformation.
            if (filter.Equals("grayscale", StringComparison.OrdinalIgnoreCase))
                image.Mutate(x => x.Grayscale());
            //example   image.Mutate(x => x.Grayscale());

            // If the requested filter is "sepia" (case-insensitive), apply sepia transformation.
            else if (filter.Equals("sepia", StringComparison.OrdinalIgnoreCase))
                image.Mutate(x => x.Sepia());
            //example   image.Mutate(x => x.Sepia());

            // Create a memory stream to store the output (processed image).
            using var outStream = new MemoryStream();

            // Save the processed image into the memory stream in PNG format.

            image.Save(outStream, new PngEncoder());
            // Return the processed image as a byte array.
            return outStream.ToArray();
        }
    }
}
