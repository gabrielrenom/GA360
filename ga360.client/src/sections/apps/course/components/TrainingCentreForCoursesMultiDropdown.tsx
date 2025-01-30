import React, { useEffect, useState } from "react";
import {
  Checkbox,
  FormControl,
  InputLabel,
  ListItemText,
  MenuItem,
  OutlinedInput,
  Select,
} from "@mui/material";
import { getTrainingCentres, getTrainingCentresByQualificationId, TrainingCentre } from "api/trainingcentreService";

interface TrainingCentreDropdownProps {
  qualificationId: any;
  value: number[]; // Array of numbers representing selected training centre IDs
  onChange: (id: number, value: number[]) => void;
}

const TrainingCentreForCoursesMultiDropdown: React.FC<TrainingCentreDropdownProps> = ({ qualificationId, value, onChange }) => {
  const [trainingCentres, setTrainingCentres] = useState<TrainingCentre[]>([]);
  const [selectedNames, setSelectedNames] = useState<string[]>([]);

  useEffect(() => {
    const fetchTrainingCentres = async () => {
      try {
        if (trainingCentres.length === 0) {
          const data = await getTrainingCentres();
          console.log("IDD", qualificationId)
          
          if (qualificationId !== null && qualificationId !== undefined && qualificationId !== "") {
            const selectedData = await getTrainingCentresByQualificationId(qualificationId);
            setSelectedNames(selectedData.map(centre => centre.name)); // Initialize selectedNames
          }
          
          setTrainingCentres(data);
        }
      } catch (error) {
        console.error("Failed to fetch training centres", error);
      }
    };
    fetchTrainingCentres();
  }, [trainingCentres, qualificationId]);

  const handleChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    const newValue = event.target.value as string[];
    setSelectedNames(newValue);

    // Map selected names to their corresponding IDs
    const selectedIds = trainingCentres
      .filter(centre => newValue.includes(centre.name))
      .map(centre => centre.id);

    onChange(qualificationId, selectedIds); // Update parent component with qualificationId and selected IDs
  };

  return (
    <FormControl sx={{ m: 1, width: 500 }}>
      <InputLabel id="training-centre-label">Training Centres</InputLabel>
      <Select
        labelId="training-centre-label"
        id="training-centre-select"
        multiple
        value={selectedNames}
        //@ts-ignore
        onChange={handleChange}
        input={<OutlinedInput label="Training Centres" />}
        renderValue={(selected) => (selected as string[]).join(", ")}
      >
        {trainingCentres.map((centre) => (
          <MenuItem key={centre.id} value={centre.name}>
            <Checkbox checked={selectedNames.indexOf(centre.name) > -1} />
            <ListItemText primary={centre.name} />
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
}

export default TrainingCentreForCoursesMultiDropdown;
