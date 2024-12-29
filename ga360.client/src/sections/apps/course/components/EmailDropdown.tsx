import React, { useState, useEffect } from 'react';
import { MenuItem, TextField, Box } from '@mui/material';
import { BasicCustomer } from 'types/customer';
import { getBasicCandidates } from 'api/customer';

interface EmailDropdownProps {
  value: string;
  onChange: (value: string) => void;
}

const EmailDropdown: React.FC<EmailDropdownProps> = ({ value, onChange }) => {
  const [emails, setEmails] = useState<BasicCustomer[]>([]);
  const [selectedValue, setSelectedValue] = useState<string>(''); // Use local state for selected value

  useEffect(() => {
    const fetchEmails = async () => {
      if (emails.length === 0) {
        try {
          const emailData = await getBasicCandidates();
          console.log("EMAIL CALLED", value);

          // Filter out duplicate emails
          const distinctEmails = emailData.filter((email, index, self) =>
            index === self.findIndex((client) => (
              client.email === email.email
            ))
          );

          setEmails(distinctEmails);

          // Set the initial selected value if it matches one of the options
          const matchedEmail = distinctEmails.find(email => email.email === value);
          if (matchedEmail) {
            setSelectedValue(matchedEmail.id.toString());
            onChange(matchedEmail.id.toString()); // Ensure parent component is updated
          }
        } catch (error) {
          console.error("Failed to fetch emails", error);
        }
      } else {
        // Set the initial selected value if it matches one of the already fetched options
        const matchedEmail = emails.find(email => email.email === value);
        if (matchedEmail) {
          setSelectedValue(matchedEmail.id.toString());
          onChange(matchedEmail.id.toString()); // Ensure parent component is updated
        }
      }
    };

    fetchEmails();
  }, [value, onChange, emails]);

  const handleChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    const newValue = event.target.value as string;
    setSelectedValue(newValue);
    onChange(newValue);
  };

  return (
    <Box display="flex" flexDirection="column" width="100%" style={{ paddingLeft: '10px' }}>
      <TextField
        select
        label="Email"
        value={selectedValue}
        onChange={handleChange}
        variant="outlined"
        fullWidth
        style={{ width: '150px', paddingTop: '10px', backgroundColor: 'white' }}
        InputLabelProps={{ style: { marginTop: '10px' } }}
      >
        {emails.map((email) => (
          <MenuItem key={email.id} value={email.id.toString()}>
            {email.email}
          </MenuItem>
        ))}
      </TextField>
    </Box>
  );
};

export default EmailDropdown;
