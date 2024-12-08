// export default MultipleFileUploader;
import React, { useState, useEffect } from 'react';
import { Button, Typography, Box, IconButton, LinearProgress } from '@mui/material';
import { styled } from '@mui/material/styles';
import FileProtectOutlined from '@ant-design/icons/FileProtectOutlined';
import CloseCircleOutlined from '@ant-design/icons/CloseCircleOutlined';
import { DocumentFileModel } from 'types/customerApiModel';

const Input = styled('input')({
  display: 'none',
});

const MAX_FILE_SIZE_MB = 25;
const ALLOWED_FILE_TYPES = ['image/jpeg', 'image/png', 'image/gif', 'application/pdf', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'];

interface MultipleFileUploaderProps {
  onFilesUpload: (files: File[]) => void;
  detailedFiles?: DocumentFileModel[];
  initialFiles?: File[]; // Add initialFiles prop
}

const MultipleFileUploader: React.FC<MultipleFileUploaderProps> = ({ onFilesUpload,detailedFiles = [], initialFiles = [] }) => {
  const [selectedFiles, setSelectedFiles] = useState<File[]>(initialFiles);
  const [error, setError] = useState<string | null>(null);
  const [uploadProgress, setUploadProgress] = useState<number>(0);
  const [isUploading, setIsUploading] = useState<boolean>(false);

  function getUrlByFileName(documents: DocumentFileModel[], fileName: string): string | null {
    const document = documents.find(doc => doc.name === fileName);
    return document ? document.url : null;
  }

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const files = Array.from(event.target.files || []);
    const validFiles: File[] = [];
    let errorMessage = '';

    files.forEach((file) => {
      if (!ALLOWED_FILE_TYPES.includes(file.type)) {
        errorMessage = 'Invalid file type. Please upload JPEG, PNG, or GIF images.';
      } else if (file.size > MAX_FILE_SIZE_MB * 1024 * 1024) {
        errorMessage = `File size exceeds ${MAX_FILE_SIZE_MB} MB. Please choose smaller files.`;
      } else {
        validFiles.push(file);
      }
    });

    if (errorMessage) {
      setError(errorMessage);
    } else {
      setSelectedFiles((prevFiles) => [...prevFiles, ...validFiles]);
      setError(null);
    }
  };

  const handleUpload = () => {
    if (selectedFiles.length > 0) {
      setIsUploading(true);
      const totalFiles = selectedFiles.length;
      let uploadedFiles = 0;

      selectedFiles.forEach((file, index) => {
        // Simulate file upload with a timeout
        setTimeout(() => {
          uploadedFiles += 1;
          setUploadProgress((uploadedFiles / totalFiles) * 100);

          if (uploadedFiles === totalFiles) {
            setIsUploading(false);
            onFilesUpload(selectedFiles);
          }
        }, 1000 * (index + 1)); // Simulate different upload times for each file
      });
    } else {
      console.error('No files selected');
    }
  };

  const handleRemoveFile = (index: number) => {
    const newFiles = [...selectedFiles];
    newFiles.splice(index, 1);
    setSelectedFiles(newFiles);
  };

  return (
    <Box p={3} border="1px dashed #ccc" borderRadius={8} textAlign="center">
      <label htmlFor="file-input">
        <Input
          id="file-input"
          type="file"
          accept="image/*"
          multiple
          onChange={handleFileChange}
        />
        <Button variant="outlined" component="span">
          Select Files
        </Button>
      </label>
      {selectedFiles.length > 0 && (
        <div>
          <ul>
            {selectedFiles.map((file, index) => (
              <span key={index}>
                <Box display="flex" alignItems="center" justifyContent="center">
                  <FileProtectOutlined />
                  <Typography variant="body2" ml={1}>
                    <a style={{ textDecoration: 'none', color:'#3b3b3b' }} href={getUrlByFileName(detailedFiles, file.name)} download={file.name} target="_blank" rel="noopener noreferrer">
                      {file.name}
                    </a>
                </Typography>
                  <IconButton style={{color:'red' }}  onClick={() => handleRemoveFile(index)} size="small">
                    <CloseCircleOutlined/>
                  </IconButton>
                </Box>
              </span>
            ))}
          </ul>
          {isUploading && (
            <Box mt={2} width="100%">
              <LinearProgress variant="determinate" value={uploadProgress} />
              <Typography variant="body2" mt={1}>
                Uploading... {Math.round(uploadProgress)}%
              </Typography>
            </Box>
          )}
          <Button variant="contained" color="primary" onClick={handleUpload} sx={{ mt: 2 }} disabled={isUploading}>
            Upload
          </Button>
        </div>
      )}
      {error && (
        <Typography variant="body2" color="error" mt={2}>
          {error}
        </Typography>
      )}
    </Box>
  );
};

export default MultipleFileUploader;