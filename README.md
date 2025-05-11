# StreetPhotoConverter  

StreetPhotoConverter is a console application that processes images in an `input` directory, applies various transformations, and saves the processed images to the `output` directory. The transformations aim to create an effect suitable for street photography.

## Features  
- Adjusts brightness, contrast, and exposure of images.  
- Applies grayscale conversion.  
- Adds random noise.  
- Supports multiple image formats: `.jpg`, `.jpeg`, `.webp`, `.png`.  

## Getting Started  

### 1. Place Input Files  
Place your image files in the `input` directory.  

### 2. Run the Application

### 3. View Output  
Processed images will be saved in the `output` directory.  

## How It Works  
1. The application scans the `input` directory for supported image files.  
2. Each image undergoes the following transformations:  
   - Brightness adjustment (+/-10% based on average luminance).  
   - Contrast boost (+50%).  
   - Exposure adjustment (+10%).  
   - Grayscale conversion.  
   - Random noise addition.
3. The processed images are saved in the `output` directory.  

## License  
This project is licensed under the MIT License. See the `LICENSE` file for details.  

## Acknowledgments  
- [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) for image processing.  
- [Spectre.Console](https://github.com/spectresystems/spectre.console) for enhanced console output.
