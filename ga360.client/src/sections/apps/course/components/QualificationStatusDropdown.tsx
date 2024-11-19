import React, { useEffect, useState } from 'react';
import { Autocomplete, TextField, Box } from '@mui/material';
import { getQualificationStatuses, QualificationStatus } from 'api/qualificationService'; // Adjust the import path

interface QualificationStatusDropdownProps {
  value: string;
  onChange: (value: string) => void;
}

const QualificationStatusDropdown: React.FC<QualificationStatusDropdownProps> = ({ value, onChange }) => {
  const [qualificationStatuses, setQualificationStatuses] = useState<QualificationStatus[]>([]);

  useEffect(() => {
    const fetchQualificationStatuses = async () => {
      try {
        const data = await getQualificationStatuses();
        setQualificationStatuses(data);
      } catch (error) {
        console.error("Failed to fetch qualification statuses", error);
      }
    };

    fetchQualificationStatuses();
  }, []);

  const handleChange = (event: any, newValue: any) => {
    onChange(newValue ? newValue.value : '');
  };

  const qualificationStatusOptions = qualificationStatuses.map(status => ({
    label: status.name,
    value: status.id.toString(),
  }));

  return (
    <Box display="flex" flexDirection="column" width="100%" style={{ paddingLeft: '10px'}}>
      <Autocomplete
        value={qualificationStatusOptions.find(option => option.value === value) || null}
        onChange={handleChange}
        options={qualificationStatusOptions}
        getOptionLabel={(option) => option.label}
        isOptionEqualToValue={(option, value) => option.value === value}
        renderInput={(params) => (
          <TextField
            {...params}
            label="Qualification Status"
            variant="outlined"
            style={{ backgroundColor: 'white', width: '150px', paddingTop:'10px'  }} // Set background color here
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

export default QualificationStatusDropdown;
