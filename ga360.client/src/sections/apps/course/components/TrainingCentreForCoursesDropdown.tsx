import React, { useEffect, useState } from 'react';
import { MenuItem, TextField, Box, Select, InputLabel, FormControl, Checkbox, ListItemText } from '@mui/material';
import { getTrainingCentres, TrainingCentre } from 'api/trainingcentreService';

interface TrainingCentreDropdownProps {
  value: string[];
  onChange: (selectedIds: string[]) => void;
}

const TrainingCentreForCoursesDropdown: React.FC<TrainingCentreDropdownProps> = ({ value, onChange }) => {
  const [trainingCentres, setTrainingCentres] = useState<TrainingCentre[]>([]);
  const [selectedValues, setSelectedValues] = useState<string[]>([]);

  useEffect(() => {
    const fetchTrainingCentres = async () => {
      try {
        const data = await getTrainingCentres();
        setTrainingCentres(data);
        const matchedTrainingCentres = data.filter(tc => value.includes(tc.id.toString()));
        setSelectedValues(matchedTrainingCentres.map(tc => tc.id.toString()));
        onChange(matchedTrainingCentres.map(tc => tc.id.toString()));
      } catch (error) {
        console.error("Failed to fetch training centres", error);
      }
    };
    fetchTrainingCentres();
  }, [value, onChange]);

  const handleChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    const newValues = event.target.value as string[];
    setSelectedValues(newValues);
    onChange(newValues);
  };

  return (
    <Box display="flex" flexDirection="column" width="100%" sx={{paddingTop: '0.4em'}}>
      <FormControl variant="outlined" fullWidth>
        <InputLabel>Training Centre</InputLabel>
        <Select
          multiple
          value={selectedValues}
          // @ts-ignore
          onChange={handleChange}
          label="Training Centre"
          renderValue={(selected) => selected.map((value) => {
            const trainingCentre = trainingCentres.find(tc => tc.id.toString() === value);
            return trainingCentre ? trainingCentre.name : value;
          }).join(', ')}
        >
          {trainingCentres.map((trainingCentre) => (
            <MenuItem key={trainingCentre.id} value={trainingCentre.id.toString()}>
              <Checkbox checked={selectedValues.indexOf(trainingCentre.id.toString()) > -1} />
              <ListItemText primary={trainingCentre.name} />
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    </Box>
  );
};

export default TrainingCentreForCoursesDropdown;
