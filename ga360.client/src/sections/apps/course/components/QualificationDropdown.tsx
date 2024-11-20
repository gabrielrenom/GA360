import React, { useState, useEffect } from 'react';
import { MenuItem, TextField, Box } from '@mui/material';
import { getQualifications, Qualification } from 'api/qualificationService';

interface QualificationDropdownProps {
  value: string;
  onChange: (value: string) => void;
}

const QualificationDropdown: React.FC<QualificationDropdownProps> = ({ value, onChange }) => {
  const [qualifications, setQualifications] = useState<Qualification[]>([]);
  const [selectedValue, setSelectedValue] = useState<string>(''); // Use local state for selected value

  useEffect(() => {
    const fetchQualifications = async () => {
      try {
        const qualificationData = await getQualifications();
        setQualifications(qualificationData);

        // Set the initial selected value if it matches one of the options
        const matchedQualification = qualificationData.find(qualification => qualification.name === value);
        if (matchedQualification) {
          setSelectedValue(matchedQualification.id.toString());
          onChange(matchedQualification.id.toString()); // Ensure parent component is updated
        }
      } catch (error) {
        console.error("Failed to fetch qualifications", error);
      }
    };

    fetchQualifications();
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
        label="Qualification"
        value={selectedValue}
        onChange={handleChange}
        variant="outlined"
        fullWidth
        style={{ width: '150px', paddingTop: '10px', backgroundColor: 'white' }}
        InputLabelProps={{ style: { marginTop: '10px' } }}
      >
        {qualifications.map((qualification) => (
          <MenuItem key={qualification.id} value={qualification.id.toString()}>
            {qualification.name}
          </MenuItem>
        ))}
      </TextField>
    </Box>
  );
};

export default QualificationDropdown;
