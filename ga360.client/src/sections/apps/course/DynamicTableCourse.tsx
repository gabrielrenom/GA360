import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import { useTheme } from '@mui/material/styles';
import {
  PlusOutlined,
  EditOutlined,
  DeleteOutlined,
  SaveOutlined,
  CloseOutlined,
} from "@ant-design/icons";
import {
  GridRowsProp,
  GridRowModesModel,
  GridRowModes,
  DataGrid,
  GridColDef,
  GridToolbarContainer,
  GridActionsCellItem,
  GridEventListener,
  GridRowId,
  GridRowModel,
  GridRowEditStopReasons,
  GridSlots,
} from "@mui/x-data-grid";
import MainCard from "components/MainCard";
import { Grid, MenuItem, Select, Stack, TextField, Tooltip, useMediaQuery } from "@mui/material";
import IconButton from "components/@extended/IconButton";
import { addCourse, Course, CourseDetails, deleteCourse, getCourses, getCoursesDetails, updateCourse } from "api/courseService";
import { useEffect, useState } from "react";
import { getTrainingCentres, TrainingCentre } from "api/trainingcentreService";
import TrainingCentreForCoursesDropdown from "./components/TrainingCentreForCoursesDropdown";
import { padding } from "@mui/system";


const initialRows: GridRowsProp = [];

interface EditToolbarProps {
  setRows: (newRows: (oldRows: GridRowsProp) => GridRowsProp) => void;
  setRowModesModel: (
    newModel: (oldModel: GridRowModesModel) => GridRowModesModel
  ) => void;
  trainingCentres: TrainingCentre[]; 
}

function EditToolbar(props: EditToolbarProps) {
  const { setRows, setRowModesModel, trainingCentres } = props;

  const handleClick = () => {
    const id = ''; // Ensure unique ID
    const defaultTrainingCentre = trainingCentres.length > 0 ? trainingCentres[0] : null;
    setRows((oldRows) => [
      {
        id,
        name: "",
        description: "",
        duration: 0,
        registrationDate: null,
        expectedDate: null,
        certificateDate: null,
        certificateNumber: "",
        status: 1,
        sector: "Technology",
        trainingCentre: defaultTrainingCentre ? defaultTrainingCentre.id.toString() : "", // Set default value
        isNew: true,
      },
      ...oldRows,
    ]);
    setRowModesModel((oldModel) => ({
      ...oldModel,
      [id]: { mode: GridRowModes.Edit, fieldToFocus: "name" },
    }));
  };
  
  return (
    <GridToolbarContainer sx={{ justifyContent: 'flex-end', paddingBottom: 2, paddingRight: 2 }}>
      <Button
        variant="contained"
        color="primary"
        startIcon={<PlusOutlined />}
        onClick={handleClick}
      >
        Add Course
      </Button>
    </GridToolbarContainer>
  );
}

export default function DynamicTableCourse() {
  const theme = useTheme();
  const downSM = useMediaQuery(theme.breakpoints.down('sm'));

  const [rows, setRows] = React.useState<GridRowsProp>(initialRows);
  const [rowModesModel, setRowModesModel] = React.useState<GridRowModesModel>({});

  const [trainingCentres, setTrainingCentres] = useState([]);

  useEffect(() => {
    async function fetchTrainingCentres() {
      const centres = await getTrainingCentres();
      setTrainingCentres(centres);
    }
    fetchTrainingCentres();
  }, []);

  React.useEffect(() => {
    const fetchCourses = async () => {
      try {
        const courses: CourseDetails[] = await getCoursesDetails();
        console.log("COURSES",courses)
        setRows(courses);
      } catch (error) {
        console.error('Failed to fetch courses:', error);
      }
    };

    fetchCourses();
  }, []);

  const handleRowEditStop: GridEventListener<"rowEditStop"> = (params, event) => {
    if (params.reason === GridRowEditStopReasons.rowFocusOut) {
      event.defaultMuiPrevented = true;
    }
  };

  const handleEditClick = (id: GridRowId) => () => {
    setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.Edit } });
  };

  const handleSaveClick = (id: GridRowId) => () => {
    setRowModesModel((prevRowModesModel) => ({
      ...prevRowModesModel,
      [id]: { mode: GridRowModes.View },
    }));
  };
  
  
  const handleDeleteClick = (id: GridRowId) => async () => {    
    try {
      await deleteCourse(Number(id));
      setRows(rows.filter((row) => row.id !== id));
    }
    catch (error){
      console.error('Failed to delete course:', error);
    }

  };

  const handleCancelClick = (id: GridRowId) => () => {
    setRowModesModel({
      ...rowModesModel,
      [id]: { mode: GridRowModes.View, ignoreModifications: true },
    });

    const editedRow = rows.find((row) => row.id === id);
    if (editedRow!.isNew) {
      setRows(rows.filter((row) => row.id !== id));
    }
  };

  
  const mapToGridRowModel = (course: CourseDetails): GridRowModel => {
    return {
      id: course.id,
      name: course.name,
      description: course.description,
      duration: course.duration,
      registrationDate: new Date(course.registrationDate),
      expectedDate: new Date(course.expectedDate),
      certificateDate: new Date(course.certificateDate),
      certificateNumber: course.certificateNumber,
      status: course.status,
      sector: course.sector,
      trainingCentre: course.trainingCentre ? course.trainingCentre.toString() : '',
      learners: course.learners,
      price: course.price
    };
  };

  const mapToCourse = (row: GridRowModel): CourseDetails => {
    return {
      id: row.id as number,
      name: row.name as string,
      description: row.description as string,
      duration: row.duration as number,
      registrationDate: row.registrationDate ? new Date(row.registrationDate as string) : null,
      expectedDate: row.expectedDate ? new Date(row.expectedDate as string) : null,
      certificateDate: row.certificateDate ? new Date(row.certificateDate as string) : null,
      certificateNumber: row.certificateNumber as string,
      status: row.status as number,
      sector: row.sector as string,
      trainingCentreId: row.trainingCentreId as number,
      trainingCentre: row.trainingCentre,
      learners: row.learners,
      price: row.price,
    };
  };
  
  const fetchCourses = async () => {
    try {
      const courses: Course[] = await getCourses();
      setRows(courses.map(mapToGridRowModel));
    } catch (error) {
      console.error('Failed to fetch courses:', error);
    }
  };

  const processRowUpdate = async (newRow: GridRowModel) => {
    const updatedRow = { ...newRow, isNew: false };
    try {
      if (newRow.isNew) {
        const course = mapToCourse(newRow);
        course.id = 0;
        const createdCourse = await addCourse(course);  
  
        if (typeof createdCourse === 'object' && createdCourse !== null && createdCourse.id > 0) {
          await fetchCourses();
        } else {
          console.error('Failed to create course: createdCourse is invalid');
        }
        // Avoid returning updatedRow when adding a new course
        return;
      } else {
        const course = mapToCourse(newRow);
        console.log(newRow)

        const trainingCentreId = Number(newRow.trainingCentre);

        if (!isNaN(trainingCentreId)) {
          // It's a number, so you can use it
          course.trainingCentreId = trainingCentreId;
        } else {
          course.trainingCentreId = trainingCentres.find(tc => tc.name === newRow.trainingCentre)?.id;
        }

        const updatedCourse = await updateCourse(course.id, course); // Get the updated course
        // const updatedRowModel = {
        //   ...mapToGridRowModel(course),
        //   trainingCentre: trainingCentres.find(tc => tc.id === course.trainingCentreId)?.name || '',
        // };
        // console.log("NEWROW", newRow);
       newRow.trainingCentre = updatedCourse.trainingCentre;
       setRows(rows.map((row) => (row.id === newRow.id ? newRow : row)));
        //return;
      }
    } catch (error) {
      console.error('Failed to update course:', error);
    }
    return updatedRow;
  };
  
  
  const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };
  

  const columns: GridColDef[] = [
    {
      field: "id",
      headerName: "#",
      editable: true,
      type: "number",
      headerAlign: 'center', // Aligns the header text to the center
      align: 'center', // Aligns the cell content to the center
    },
    {
      field: "name",
      headerName: "NAME",
      flex: 1,
      editable: true,
    },
    {
      field: "description",
      headerName: "DESCRIPTION",
      flex: 1,
      editable: true,
    },
    {
      field: "duration",
      headerName: "HOURS",
      type: "number",
      flex: 1,
      editable: true,
      renderCell: (params) => {
        return <div>{params.value || 0}</div>; // Render a simple div for cell display
      },
      renderEditCell: (params) => {
        return (
          <TextField
            type="number"
            inputProps={{
              min: 0,
              inputMode: 'numeric',
              pattern: '[0-9]*'
            }}
            value={params.value || ''}
            onChange={(event) => {
              params.api.setEditCellValue({ ...params, id: params.id, field: params.field, value: event.target.value });
            }}
            sx={{
              paddingTop: '0.4em',
              width: '100px',
              paddingRight: '0.6em',
            }}
          />
        );
      }
    },
    {
      field: "learners",
      headerName: "LEARNERS",
      type: "number",
      flex: 1,
      editable: false,
    },
    {
      field: 'trainingCentre',
      headerName: 'TRAINING CENTRE',
      flex: 2,
      editable: true,
      renderEditCell: (params) => (
        <TrainingCentreForCoursesDropdown
          value={params.value}
          onChange={(value) =>
            params.api.setEditCellValue({ id: params.id, field: params.field, value }, event)
          }
        />
      ),
    },
    {
      field: "expectedDate",
      headerName: "EXPIRY DATE",
      type: "date",
      flex: 1,
      editable: true,
      valueFormatter: (params) => { const date = new Date(params as string); return date.toLocaleDateString(); },
    },
    {
      field: "status",
      headerName: "STATUS",
      flex: 1,
      editable: true,
      type: "singleSelect",
      valueOptions: [
        { value: 1, label: "Active" },
        { value: 2, label: "Not Active" },
      ],
    },
    {
      field: "sector",
      headerName: "SECTOR",
      flex: 1,
      editable: true,
      type: "singleSelect",
      valueOptions: [
          { value: "Technology", label: "Technology" },
          { value: "Healthcare", label: "Healthcare" },
          { value: "Finance", label: "Finance" },
          { value: "Education", label: "Education" },
          { value: "Engineering", label: "Engineering" },
          { value: "Marketing", label: "Marketing" },
          { value: "Hospitality", label: "Hospitality" },
          { value: "Retail", label: "Retail" },
          { value: "Manufacturing", label: "Manufacturing" },
          { value: "Construction", label: "Construction" },
      ],
  },
  {
    field: "price",
  headerName: "PRICE",
  type: "number",
  flex: 1,
  editable: true,
  renderCell: (params) => {
    return <div>{params.value || 0}</div>; // Render a simple div for cell display
  },
  renderEditCell: (params) => {
    return (
      <TextField
        type="number"
        inputProps={{
          min: 0,
          inputMode: 'numeric',
          pattern: '[0-9]*'
        }}
        value={params.value || ''}
        onChange={(event) => {
          params.api.setEditCellValue({ ...params, id: params.id, field: params.field, value: event.target.value });
        }}
        sx={{
          paddingTop: '0.4em',
          width: '100px',
          paddingRight: '0.6em',
        }}
      />
    );
  },

    // preProcessEditCellProps: (params) => {
    //   return {
    //     inputProps: {
    //       min: 0, 
    //       // type: 'number', 
    //       // inputMode: 'numeric', 
    //       // pattern: '[0-9]*' 
    //     }
    //   };
    // },
  },
    {
        field: "actions",
        type: "actions",
        headerName: "ACTIONS",
        flex: 1,
        cellClassName: "actions",
        getActions: ({ id }) => {
          const isInEditMode = rowModesModel[id]?.mode === GridRowModes.Edit;
      
          if (isInEditMode) {
            return [
              <GridActionsCellItem
                icon={<SaveOutlined />}
                label="Save"
                sx={{ color: "primary.main" }}
                onClick={handleSaveClick(id)}
              />,
              <GridActionsCellItem
                icon={<CloseOutlined />}
                label="Cancel"
                className="textPrimary"
                onClick={handleCancelClick(id)}
                color="inherit"
              />,
            ];
          }
      
          return [
            <GridActionsCellItem
              icon={
                <Tooltip title="Edit">
                  <IconButton color="primary">
                    <EditOutlined />
                  </IconButton>
                </Tooltip>
              }
              label="Edit"
              className="textPrimary"
              onClick={handleEditClick(id)}
              color="inherit"
            />,
            <GridActionsCellItem
              icon={
                <Tooltip title="Delete">
                  <IconButton color="error">
                    <DeleteOutlined />
                  </IconButton>
                </Tooltip>
              }
              label="Delete"
              onClick={handleDeleteClick(id)}
              color="inherit"
            />,
          ];
        },
      }
    ]
    return (
        <MainCard content={false}>
          <Stack
            direction={{ xs: 'column', sm: 'row' }}
            spacing={2}
            alignItems="center"
            justifyContent="space-between"
            sx={{ paddingTop: 2, ...(downSM && { '& .MuiOutlinedInput-root, & .MuiFormControl-root': { width: '100%' } }) }}
          >
            <Box
              sx={{
                height: 500,
                width: "100%",
                overflowX: "auto", // Enable horizontal scrolling
                "& .actions": {
                  color: "text.secondary",
                },
                "& .textPrimary": {
                  color: "text.primary",
                },
              }}
            >
              <DataGrid
                rows={rows}
                columns={columns}
                editMode="row"
                rowModesModel={rowModesModel}
                onRowModesModelChange={handleRowModesModelChange}
                onRowEditStop={handleRowEditStop}
                processRowUpdate={processRowUpdate}
                disableRowSelectionOnClick 
                onCellDoubleClick={(params, event) => { event.stopPropagation(); }}
                slots={{
                  toolbar: EditToolbar as GridSlots['toolbar'],
                }}
                slotProps={{
                  toolbar: { setRows, setRowModesModel, trainingCentres },
                }}
                sx={{
                  '& .MuiDataGrid-columnHeaders': {
                    borderBottom: `1px solid ${theme.palette.divider}`,
                    borderTop: `1px solid ${theme.palette.divider}`,
                    padding: 0, // Remove padding from the header
                    '& .MuiDataGrid-columnHeaderTitle': {
                      fontWeight: 'bold',
                    },
                    '& .MuiDataGrid-columnHeaderTitle:first-child': {
                      paddingLeft: 1, // Set the padding of the first column header
                    },  
                  },
                  '& .MuiDataGrid-cell': {
                    borderColor: theme.palette.divider,
                    padding: theme.spacing(1), // Add padding to the rows
                    paddingLeft: 2,
                  },
                  "& .MuiDataGrid-pagination": {
                    borderTop: "1px solid",
                    marginTop: theme.spacing(2), // Add some margin for better separation
                  },
                }}
              />
            </Box>
          </Stack>
        </MainCard>
      );
    }      