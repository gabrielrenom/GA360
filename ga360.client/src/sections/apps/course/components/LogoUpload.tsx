import React, { useState, useEffect, memo } from 'react';
import { Avatar, Box, FormLabel, Stack } from '@mui/material';
import CameraOutlined from "@ant-design/icons/CameraOutlined";
import { GridRowParams } from '@mui/x-data-grid';

interface LogoUploadProps {
  initialLogo?: string;
  onFileUpload: (file: File, setImageUrl: React.Dispatch<React.SetStateAction<string | undefined>>, id: GridRowParams['id']) => void;
  id: GridRowParams['id']; // Add id prop
}

const LogoUpload: React.FC<LogoUploadProps> = ({ initialLogo, onFileUpload, id }) => {
  const [imageUrl, setImageUrl] = useState<string | undefined>(initialLogo);

  useEffect(() => {
    setImageUrl(initialLogo);
  }, [initialLogo]);

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      onFileUpload(file, setImageUrl, id); // Pass id to onFileUpload
    }
  };

  return (
    <Stack direction="row" justifyContent="center" sx={{ mt: 0.5, mr: 5 }}>
      <FormLabel
        htmlFor="change-logo"
        sx={{
          position: 'relative',
          borderRadius: '50%',
          overflow: 'hidden',
          '&:hover .MuiBox-root': { opacity: 1 },
          cursor: 'pointer',
        }}
      >
        <Box
          sx={{
            width: '100%',
            height: '100%',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            position: 'relative',
            borderRadius: '50%',
          }}
        >
          <CameraOutlined style={{ fontSize: '2rem' }} />
          {imageUrl && (
            <img
              src={imageUrl}
              alt="Uploaded logo"
              style={{
                position: 'absolute',
                top: 0,
                left: 0,
                width: '100%',
                height: '100%',
                borderRadius: '50%',
                objectFit: 'cover',
              }}
            />
          )}
        </Box>
      </FormLabel>
      <input
        type="file"
        id="change-logo"
        style={{ display: 'none' }}
        accept="image/*"
        onChange={handleFileChange}
      />
    </Stack>
  );
};

export default memo(LogoUpload);