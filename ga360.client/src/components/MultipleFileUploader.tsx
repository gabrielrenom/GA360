import React, { useState } from 'react';
import { Button, Typography, Box, IconButton } from '@mui/material';
import { styled } from '@mui/material/styles';
import FileProtectOutlined from '@ant-design/icons/FileProtectOutlined';
import CloseCircleOutlined from '@ant-design/icons/CloseCircleOutlined';

const Input = styled('input')({
  display: 'none',
});

const MAX_FILE_SIZE_MB = 25;
const ALLOWED_FILE_TYPES = ['image/jpeg', 'image/png', 'image/gif', 'application/pdf', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'];

interface MultipleFileUploaderProps {
  onFilesUpload: (files: File[]) => void;
}

const MultipleFileUploader: React.FC<MultipleFileUploaderProps> = ({ onFilesUpload }) => {
  const [selectedFiles, setSelectedFiles] = useState<File[]>([]);
  const [error, setError] = useState<string | null>(null);

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
      setSelectedFiles(validFiles);
      setError(null);
    }
  };

  const handleUpload = () => {
    if (selectedFiles.length > 0) {
      onFilesUpload(selectedFiles);
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
                  <Typography variant="body2" ml={1}>{file.name}</Typography>
                  <IconButton onClick={() => handleRemoveFile(index)} size="small">
                    <CloseCircleOutlined/>
                  </IconButton>
                </Box>
              </span>
            ))}
          </ul>
          <Button variant="contained" color="primary" onClick={handleUpload} mt={2}>
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
