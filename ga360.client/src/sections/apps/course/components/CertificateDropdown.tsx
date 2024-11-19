import React, { useState, useEffect } from 'react';
import { Autocomplete, TextField, Box } from '@mui/material';
import { Certificate, getCertificates } from 'api/certificateService';

interface CertificateDropdownProps {
  value: string;
  onChange: (value: string) => void;
}

const CertificateDropdown: React.FC<CertificateDropdownProps> = ({ value, onChange }) => {
  const [certificates, setCertificates] = useState<Certificate[]>([]);

  useEffect(() => {
    const fetchCertificates = async () => {
      try {
        const data = await getCertificates();
        setCertificates(data);
      } catch (error) {
        console.error("Failed to fetch certificates", error);
      }
    };

    fetchCertificates();
  }, []);

  const certificateOptions = certificates.map(cert => ({
    label: cert.name,
    value: cert.id.toString(),
  }));

  const handleChange = (event: any, newValue: any) => {
    onChange(newValue ? newValue.value : '');
  };

  return (
    <Box display="flex" flexDirection="column" width="100%" style={{ paddingLeft: '10px' }}>
      <Autocomplete
        value={certificateOptions.find(option => option.value === value) || null}
        onChange={handleChange}
        options={certificateOptions}
        getOptionLabel={(option) => option.label}
        isOptionEqualToValue={(option, value) => option.value === value}
        renderInput={(params) => (
          <TextField
            {...params}
            label="Certificate"
            variant="outlined"
            fullWidth
            style={{ width: '150px', paddingTop: '10px', backgroundColor: 'white' }} // Set background color here
            InputLabelProps={{ style: { marginTop: '10px' } }} // Adjust label margin here
          />
        )}
        PaperComponent={(props) => (
          <div {...props} style={{ ...props.style, maxWidth: '100%', minWidth: '300px', backgroundColor: 'white' }} /> // Set background color here
        )}
        style={{ width: '300px', flex: 1 }}
      />
    </Box>
  );
};

export default CertificateDropdown;
