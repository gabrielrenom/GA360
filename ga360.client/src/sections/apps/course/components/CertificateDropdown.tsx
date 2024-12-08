import React, { useState, useEffect } from 'react';
import { MenuItem, TextField, Box } from '@mui/material';
import { Certificate, getCertificates } from 'api/certificateService';

interface CertificateDropdownProps {
  value: string;
  onChange: (value: string) => void;
}

const CertificateDropdown: React.FC<CertificateDropdownProps> = ({ value, onChange }) => {
  const [certificates, setCertificates] = useState<Certificate[]>([]);
  const [selectedValue, setSelectedValue] = useState<string>(''); // Use local state for selected value

  useEffect(() => {
    const fetchCertificates = async () => {
      try {
        const data = await getCertificates();
        setCertificates(data);
        console.log("fetchCertificates CALLED");

        // Set the initial selected value if it matches one of the options
        const matchedCertificate = data.find(cert => cert.name === value);
        if (matchedCertificate) {
          setSelectedValue(matchedCertificate.id.toString());
          onChange(matchedCertificate.id.toString()); // Ensure parent component is updated
        }
      } catch (error) {
        console.error("Failed to fetch certificates", error);
      }
    };

    fetchCertificates();
  }, [value, onChange]);

  const handleChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    const newValue = event.target.value as string;
    setSelectedValue(newValue);
    onChange(newValue);
  };

  return (
    <Box display="flex" flexDirection="column" width="100%" style={{ paddingLeft: '10px' }}>
      <TextField
        select
        label="Certificate"
        value={selectedValue}
        onChange={handleChange}
        variant="outlined"
        fullWidth
        style={{ width: '150px', paddingTop: '10px', backgroundColor: 'white' }}
        InputLabelProps={{ style: { marginTop: '10px' } }}
      >
        {certificates.map((certificate) => (
          <MenuItem key={certificate.id} value={certificate.id.toString()}>
            {certificate.name}
          </MenuItem>
        ))}
      </TextField>
    </Box>
  );
};

export default CertificateDropdown;
