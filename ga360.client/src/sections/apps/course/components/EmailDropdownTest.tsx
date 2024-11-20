import React, { useState, useEffect } from 'react';
import { Autocomplete, TextField, Box } from '@mui/material';
import { BasicCustomer } from 'types/customer';
import { getBasicCandidates } from 'api/customer';

interface EmailDropdownProps {
  value: string;
  onChange: (value: string) => void;
}

const EmailDropdownTest: React.FC<EmailDropdownProps> = ({ value, onChange }) => {
  const [emails, setEmails] = useState<BasicCustomer[]>([]);

  useEffect(() => {
    const fetchEmails = async () => {
      try {
        const emailData = await getBasicCandidates();
        console.log("EMAIL CALLED",value);
  
        // Filter out duplicate emails
        const distinctEmails = emailData.filter((value, index, self) =>
          index === self.findIndex((client) => (
            client.email === value.email
          ))
        );
  
        setEmails(distinctEmails);
      } catch (error) {
        console.error("Failed to fetch emails", error);
      }
    };
    console.log("EMAIL CALLED",value);

    fetchEmails();

  }, []);
  

  const emailOptions = emails.map(email => ({
    label: email.email,
    value: email.id.toString(),
  }));

  const handleChange = (event: any, newValue: any) => {
    console.log('Selected value:', newValue ? newValue.value : '');
    onChange(newValue ? newValue.value : '');
  };

  // Use the current value or fallback to the first option as a default
  const defaultValue = emailOptions.find(option => option.label === value) || null;

  return (
    <Box display="flex" flexDirection="column" width="100%" style={{ paddingLeft: '10px' }}>
      <Autocomplete
        value={defaultValue}
        onChange={handleChange}
        options={emailOptions}
        getOptionLabel={(option) => option.label}
        isOptionEqualToValue={(option, value) => option.label === value}
        renderInput={(params) => (
          <TextField
            {...params}
            label="Email"
            variant="outlined"
            style={{ width: '150px', paddingTop: '10px', backgroundColor: 'white' }}
            InputLabelProps={{ style: { marginTop: '10px' } }}
          />
        )}
        PaperComponent={(props) => (
          <div {...props} style={{ ...props.style, maxWidth: '100%', minWidth: '300px', backgroundColor: 'white' }} />
        )}
        style={{ width: '300px', flex: 1 }}
      />
    </Box>
  );
};

export default EmailDropdown;
