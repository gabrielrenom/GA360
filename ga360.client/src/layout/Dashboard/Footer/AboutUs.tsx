import React from 'react';
import Paper from '@mui/material/Paper';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';

export default function AboutUs() {
  return (
    <Box sx={{ p: 3 }}>
      <Paper elevation={3} sx={{ p: 2 }}>
        <Typography variant="h5" gutterBottom>
          About Us
        </Typography>
        <Typography variant="body1" paragraph>
          Global Alliance 360 LLC is a pioneering collaboration between two leading UK-based organizations: Coburns European Training and GQA. Together, they bring decades of expertise in vocational and management training to a global audience, delivering top-tier education and certification services.
        </Typography>
        <Typography variant="body1" paragraph>
          <strong>Established Expertise:</strong> Coburns European Training has been a prominent UK vocational training provider for the past 12 years, specializing in sectors like construction, health and safety, and the beauty industry.
        </Typography>
        <Typography variant="body1" paragraph>
          <strong>Nationwide Reach:</strong> They operate 35 training centers across the UK, ensuring accessibility and convenience for all students.
        </Typography>
        <Typography variant="body1" paragraph>
          <strong>Regulatory Authority:</strong> GQA, with 15 years of experience, stands as a respected regulatory body in the vocational training sector, with 95 registered centers throughout the UK.
        </Typography>
        <Typography variant="body1" paragraph>
          <strong>Notable Projects and Collaborations:</strong> They have spearheaded transformative projects across diverse industries, including delivering health and safety certifications in over 60 UK prisons and collaborating on gas and oil qualifications in Saudi Arabia.
        </Typography>
        <Typography variant="body1" paragraph>
          <strong>Global Presence:</strong> They have developed a network of academies in the UK and Eastern Europe, with strategic partners in Moldova and Romania.
        </Typography>
      </Paper>
    </Box>
  );
}
