import React, { useEffect, useState } from 'react';
import { MenuItem, TextField, Box } from '@mui/material';
import { getQualificationStatuses, QualificationStatus } from 'api/qualificationService'; // Adjust the import path

interface QualificationStatusDropdownProps {
  value: string;
  onChange: (value: string) => void;
}

const QualificationStatusDropdown: React.FC<QualificationStatusDropdownProps> = ({ value, onChange }) => {
  const [qualificationStatuses, setQualificationStatuses] = useState<QualificationStatus[]>([]);
  const [selectedValue, setSelectedValue] = useState<string>(''); // Use local state for selected value

  useEffect(() => {
    const fetchQualificationStatuses = async () => {
      try {
        const data = await getQualificationStatuses();
        setQualificationStatuses(data);

        // Set the initial selected value if it matches one of the options
        const matchedQualificationStatus = data.find(status => status.name === value);
        if (matchedQualificationStatus) {
          setSelectedValue(matchedQualificationStatus.id.toString());
          onChange(matchedQualificationStatus.id.toString()); // Ensure parent component is updated
        }
      } catch (error) {
        console.error("Failed to fetch qualification statuses", error);
      }
    };

    fetchQualificationStatuses();
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
        label="Qualification Status"
        value={selectedValue}
        onChange={handleChange}
        variant="outlined"
        fullWidth
        style={{ width: '150px', paddingTop: '10px', backgroundColor: 'white' }} // Set background color here
        InputLabelProps={{ style: { marginTop: '10px' } }} // Adjust label margin here
      >
        {qualificationStatuses.map((status) => (
          <MenuItem key={status.id} value={status.id.toString()}>
            {status.name}
          </MenuItem>
        ))}
      </TextField>
    </Box>
  );
};

export default QualificationStatusDropdown;
