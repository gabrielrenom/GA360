import React from 'react';
import Paper from '@mui/material/Paper';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';

export default function Privacy() {
  return (
    <Box sx={{ p: 3 }}>
      <Paper elevation={3} sx={{ p: 2 }}>
        <Typography variant="h5" gutterBottom>
          Privacy Policy
        </Typography>
        <Typography variant="body1" paragraph>
          At Global Alliance 360, we are committed to protecting your privacy. This Privacy Policy outlines how we collect, use, disclose, and safeguard your information when you visit our website or use our services.
        </Typography>
        
        <Typography variant="h6" gutterBottom>
          Information We Collect
        </Typography>
        <Typography variant="body1" paragraph>
          We may collect personal information that you voluntarily provide to us when you register on the website, make a purchase, or contact us. This includes your name, email address, phone number, and payment information.
        </Typography>
        <Typography variant="body1" paragraph>
          We may also collect non-personal information about your visit to our website, such as your IP address, browser type, operating system, and browsing behavior.
        </Typography>
        
        <Typography variant="h6" gutterBottom>
          How We Use Your Information
        </Typography>
        <Typography variant="body1" paragraph>
          We use your personal information to provide you with the services you request, process your transactions, send you promotional materials, and improve our website and services.
        </Typography>
        <Typography variant="body1" paragraph>
          We may use non-personal information to analyze trends, administer the site, track users' movements around the site, and gather demographic information.
        </Typography>
        
        <Typography variant="h6" gutterBottom>
          Disclosure of Your Information
        </Typography>
        <Typography variant="body1" paragraph>
          We may share your information with third parties to facilitate the services you request, such as payment processors and delivery services. We may also disclose your information if required by law or to protect our rights.
        </Typography>
        
        <Typography variant="h6" gutterBottom>
          Cookies and Tracking Technologies
        </Typography>
        <Typography variant="body1" paragraph>
          Our website may use cookies and similar tracking technologies to enhance your experience. You can set your browser to refuse all or some cookies, or to alert you when cookies are being sent.
        </Typography>
        
        <Typography variant="h6" gutterBottom>
          Data Security
        </Typography>
        <Typography variant="body1" paragraph>
          We use administrative, technical, and physical security measures to protect your personal information. However, no method of transmission over the Internet or electronic storage is 100% secure.
        </Typography>
        
        <Typography variant="h6" gutterBottom>
          Changes to This Privacy Policy
        </Typography>
        <Typography variant="body1" paragraph>
          We may update this Privacy Policy from time to time to reflect changes in our practices. We will notify you of any significant changes by posting the new Privacy Policy on our website.
        </Typography>
        
        <Typography variant="h6" gutterBottom>
          Contact Us
        </Typography>
        <Typography variant="body1" paragraph>
          If you have any questions or concerns about this Privacy Policy or our data practices, please contact us at [contact email or phone number].
        </Typography>
      </Paper>
    </Box>
  );
}
