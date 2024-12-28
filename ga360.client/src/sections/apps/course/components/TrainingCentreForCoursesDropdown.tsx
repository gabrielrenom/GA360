import React, { useEffect, useState } from 'react';
import { MenuItem, TextField, Box } from '@mui/material';
import { getTrainingCentres, TrainingCentre } from 'api/trainingcentreService';

interface TrainingCentreDropdownProps {
  value: string;
  onChange: (id: string, name: string) => void;
}

const TrainingCentreForCoursesDropdown: React.FC<TrainingCentreDropdownProps> = ({ value, onChange }) => {
  const [trainingCentres, setTrainingCentres] = useState<TrainingCentre[]>([]);
  const [selectedValue, setSelectedValue] = useState<string>('');

  useEffect(() => {
    const fetchTrainingCentres = async () => {
      try {
        if (trainingCentres.length === 0) {
          console.log("FETCHING TRAINING");
          const data = await getTrainingCentres();
          setTrainingCentres(data);
          const matchedTrainingCentre = data.find(tc => tc.id.toString() === value);
          if (matchedTrainingCentre) {
            setSelectedValue(matchedTrainingCentre.id.toString());
            onChange(matchedTrainingCentre.id.toString(), matchedTrainingCentre.name);
          }
        } else {
          const matchedTrainingCentre = trainingCentres.find(tc => tc.id.toString() === value);
          if (matchedTrainingCentre) {
            setSelectedValue(matchedTrainingCentre.id.toString());
            onChange(matchedTrainingCentre.id.toString(), matchedTrainingCentre.name);
          }
        }
      } catch (error) {
        console.error("Failed to fetch training centres", error);
      }
    };
    fetchTrainingCentres();
  }, [value, onChange, trainingCentres]);


  const handleChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    const newValue = event.target.value as string;
    const trainingCentre = trainingCentres.find(tc => tc.id.toString() === newValue);
    setSelectedValue(newValue);
    onChange(newValue, trainingCentre ? trainingCentre.name : '');
  };

  return (
    <Box display="flex" flexDirection="column" width="100%">
      <TextField
        select
        label="Training Centre"
        value={selectedValue}
        onChange={handleChange}
        variant="outlined"
        fullWidth
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
export default TrainingCentreForCoursesDropdown;