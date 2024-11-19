import React, { useEffect, useState } from 'react';
import { Autocomplete, TextField, Box } from '@mui/material';
import { getTrainingCentres, TrainingCentre } from 'api/trainingcentreService';

interface TrainingCentreDropdownProps {
  value: string;
  onChange: (value: string) => void;
}

const TrainingCentreDropdown: React.FC<TrainingCentreDropdownProps> = ({ value, onChange }) => {
  const [trainingCentres, setTrainingCentres] = useState<TrainingCentre[]>([]);

  useEffect(() => {
    const fetchTrainingCentres = async () => {
      try {
        const data = await getTrainingCentres();
        setTrainingCentres(data);
      } catch (error) {
        console.error("Failed to fetch training centres", error);
      }
    };

    fetchTrainingCentres();
  }, []);

  const handleChange = (event: any, newValue: any) => {
    onChange(newValue ? newValue.value : '');
  };

  const trainingCentreOptions = trainingCentres.map(tc => ({
    label: tc.name,
    value: tc.id.toString(),
  }));

  return (
    <Box display="flex" flexDirection="column" width="100%" style={{ paddingLeft: '10px' }}>
      <Autocomplete
        value={trainingCentreOptions.find(option => option.value === value) || null}
        onChange={handleChange}
        options={trainingCentreOptions}
        getOptionLabel={(option) => option.label}
        isOptionEqualToValue={(option, value) => option.value === value}
        renderInput={(params) => (
          <TextField
            {...params}
            label="Training Centre"
            variant="outlined"
            fullWidth
            style={{ width: '150px', paddingTop: '10px', backgroundColor: 'white' }}
            InputLabelProps={{ style: { marginTop: '10px' } }}
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

export default TrainingCentreDropdown;
