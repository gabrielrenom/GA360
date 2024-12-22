import React from 'react';
import Paper from '@mui/material/Paper';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';

export default function Terms() {
  return (
    <Box sx={{ p: 3 }}>
      <Paper elevation={3} sx={{ p: 2 }}>
        <Typography variant="h5" gutterBottom>
          Terms and Policies
        </Typography>
        <Typography variant="body1" paragraph>
          Welcome to Global Alliance 360. This document outlines the terms and policies that govern your use of our services.
        </Typography>
        <Typography variant="body1" paragraph>
          <strong>Privacy Policy:</strong> We are committed to safeguarding the privacy of our users. Our privacy policy explains how we collect, use, and protect your personal information.
        </Typography>
        <Typography variant="body1" paragraph>
          <strong>Terms of Service:</strong> By using our services, you agree to comply with our terms of service. These terms outline your rights and responsibilities as a user.
        </Typography>
        <Typography variant="body1" paragraph>
          <strong>Cookie Policy:</strong> Our website uses cookies to enhance your browsing experience. Our cookie policy provides detailed information on how we use cookies and how you can manage them.
        </Typography>
        <Typography variant="body1" paragraph>
          <strong>Data Protection:</strong> We adhere to strict data protection regulations to ensure the security of your personal data. Our data protection policy outlines our commitment to data security.
        </Typography>
        <Typography variant="body1" paragraph>
          <strong>Contact Us:</strong> If you have any questions or concerns regarding our policies, please feel free to contact us. We are here to help you.
        </Typography>
      </Paper>
    </Box>
  );
}
