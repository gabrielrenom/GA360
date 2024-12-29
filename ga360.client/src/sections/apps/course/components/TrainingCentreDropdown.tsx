import React, { useEffect, useState } from 'react';
import { MenuItem, TextField, Box } from '@mui/material';
import { getTrainingCentres, TrainingCentre } from 'api/trainingcentreService';

interface TrainingCentreDropdownProps {
  value: string;
  onChange: (value: string) => void;
}

const TrainingCentreDropdown: React.FC<TrainingCentreDropdownProps> = ({ value, onChange }) => {
  const [trainingCentres, setTrainingCentres] = useState<TrainingCentre[]>([]);
  const [selectedValue, setSelectedValue] = useState<string>(''); // Use local state for selected value

  useEffect(() => {
    const fetchTrainingCentres = async () => {
      if (trainingCentres.length === 0) {
        try {
          const data = await getTrainingCentres();
          setTrainingCentres(data);

          // Set the initial selected value if it matches one of the options
          const matchedTrainingCentre = data.find(tc => tc.name === value);
          if (matchedTrainingCentre) {
            setSelectedValue(matchedTrainingCentre.id.toString());
            onChange(matchedTrainingCentre.id.toString()); // Ensure parent component is updated
          }
        } catch (error) {
          console.error("Failed to fetch training centres", error);
        }
      } else {
        // Set the initial selected value if it matches one of the already fetched options
        const matchedTrainingCentre = trainingCentres.find(tc => tc.name === value);
        if (matchedTrainingCentre) {
          setSelectedValue(matchedTrainingCentre.id.toString());
          onChange(matchedTrainingCentre.id.toString()); // Ensure parent component is updated
        }
      }
    };

    fetchTrainingCentres();
  }, [value, onChange, trainingCentres]);

  const handleChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    const newValue = event.target.value as string;
    setSelectedValue(newValue);
    onChange(newValue);
  };

  return (
    <Box display="flex" flexDirection="column" width="100%" style={{ paddingLeft: '10px' }}>
      <TextField
        select
        label="Training Centre"
        value={selectedValue}
        onChange={handleChange}
        variant="outlined"
        fullWidth
        style={{ width: '150px', paddingTop: '10px', backgroundColor: 'white' }}
        InputLabelProps={{ style: { marginTop: '10px' } }}
      >
        {trainingCentres.map((trainingCentre) => (
          <MenuItem key={trainingCentre.id} value={trainingCentre.id.toString()}>
            {trainingCentre.name}
          </MenuItem>
        ))}
      </TextField>
    </Box>
  );
};

export default TrainingCentreDropdown;