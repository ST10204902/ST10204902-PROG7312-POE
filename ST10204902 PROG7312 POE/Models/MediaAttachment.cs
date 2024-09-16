﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10204902_PROG7312_POE.Models
{
    /// <summary>
    /// MediaAttachment class. Represents a media attachment that can be added to an issue.
    /// </summary>
    public class MediaAttachment
    {
        //---------------------------------------------------------
        //Variables
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public Type FileType { get; private set; }

        //---------------------------------------------------------
        /// <summary>
        /// Parameterized constructor. Initializes the media attachment with the file name, file path and file type.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileType"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public MediaAttachment(string fileName, string filePath, Type fileType)
        {
            if (!ValidateFileType(filePath))
            {
                throw new InvalidOperationException("Invalid file type. Only image and document files are allowed.");
            }

            FileName = fileName;
            FilePath = filePath;
            FileType = fileType;
        }

        //---------------------------------------------------------
        /// <summary>
        /// Validates the file type of the media attachment. 
        /// This will only throw if the user somehow manages to select an invalid file type
        /// despite the filter on the OpenFileDialog.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool ValidateFileType(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath);
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".docx", ".txt" };
            return allowedExtensions.Contains(extension.ToLower());
        }
    } 
}

    