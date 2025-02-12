import { useEffect, useState, ChangeEvent, useContext } from "react";

// material-ui
import { useTheme } from "@mui/material/styles";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import CircularProgress from '@mui/material/CircularProgress';
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import Divider from "@mui/material/Divider";
import FormControl from "@mui/material/FormControl";
import FormControlLabel from "@mui/material/FormControlLabel";
import FormLabel from "@mui/material/FormLabel";
import Grid from "@mui/material/Grid";
import FormHelperText from "@mui/material/FormHelperText";
import InputLabel from "@mui/material/InputLabel";
import ListItemText from "@mui/material/ListItemText";
import MenuItem from "@mui/material/MenuItem";
import OutlinedInput from "@mui/material/OutlinedInput";
import Select, { SelectChangeEvent } from "@mui/material/Select";
import Stack from "@mui/material/Stack";
import Switch from "@mui/material/Switch";
import TextField from "@mui/material/TextField";
import Tooltip from "@mui/material/Tooltip";
import Typography from "@mui/material/Typography";
import RadioGroup from "@mui/material/RadioGroup";
import Radio from "@mui/material/Radio";
import Autocomplete from "@mui/material/Autocomplete";
import Chip from "@mui/material/Chip";

import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";

// third-party
import _ from "lodash";
import * as Yup from "yup";
import { useFormik, Form, FormikProvider } from "formik";

// project imports
import AlertCustomerDelete from "./AlertCustomerDelete";
import Avatar from "components/@extended/Avatar";
import IconButton from "components/@extended/IconButton";
import CircularWithPath from "components/@extended/progress/CircularWithPath";

import { ThemeMode, Gender } from "config";
import { openSnackbar } from "api/snackbar";
import {
    insertCustomer,
    insertCustomerWithDocuments,
    updateCustomer,
    updateCustomerWithDocuments,
} from "api/customer";
import { getImageUrl, ImagePath } from "utils/getImageUrl";

// assets
import CameraOutlined from "@ant-design/icons/CameraOutlined";
import CloseOutlined from "@ant-design/icons/CloseOutlined";
import DeleteFilled from "@ant-design/icons/DeleteFilled";

// types
import { SnackbarProps } from "types/snackbar";
import { CustomerList, CustomerListExtended, LeadListExtended } from "types/customer";
// import { TrainingCentre } from "types/trainingcentretypes";
import { getTrainingCentres, TrainingCentre } from "api/trainingcentreService";
import { getEthnicities } from "api/ethnicity";
import { Ethnicity } from "types/ethnicity";
import { getSkills } from "api/skillservice";
import { Skill } from "types/skill";
import { getCountries } from "api/countryService";
import { Country } from "types/country";
import MultiFileUpload from "components/third-party/dropzone/MultiFile";
import MultipleFileUploader from "components/MultipleFileUploader";
import { DocumentFileModel } from "types/customerApiModel";
import DuendeContext from "contexts/DuendeContext";
import { Tab, Table, TableBody, TableCell, TableHead, TableRow, Tabs } from "@mui/material";
import DynamicTableCustomerWithCourseQualificationRecords from "../course/DynamicTableCustomerWithCourseQualificationRecords";

interface StatusProps {
    value: number;
    label: string;
}

// constant
const getInitialValues = (customer: CustomerListExtended | null) => {
    const newCustomer = {
        firstName: "",
        lastName: "",
        name: "",
        email: "",
        age: 18,
        avatar: 1,
        gender: Gender.FEMALE,
        role: "",
        fatherName: "",
        orders: 0,
        progress: 50,
        status: 2,
        orderStatus: "",
        contact: "",
        country: "",
        location: "",
        about: "",
        skills: [],
        time: ["just now"],
        date: "",
        avatarImage: "",
        dateOfBirth: "",
        ethnicity: "",
        disability: "",
        employeeStatus: "",
        employer: "",
        trainingCentre: 0,
        nationalInsurance: "",
        portfolio: "",
        dob: "",
        street: "",
        city: "",
        number: "",
        postcode: "",
        documents: [],
        fileDocuments: [],
        trainingCentreId:0,
        appointmentDate: "",
        appointmentTime: "",
    };

    if (customer) {
        return _.merge({}, newCustomer, customer);
    }

    return newCustomer;
};

const allStatus: StatusProps[] = [
    { value: 3, label: "Rejected" },
    { value: 1, label: "Verified" },
    { value: 2, label: "Pending" },
];

// ==============================|| CUSTOMER ADD / EDIT - FORM ||============================== //

export default function FormLeadsAdd({
    customer,
    closeModal,
}: {
    customer: LeadListExtended | null;
    closeModal: () => void;
}) {
    const theme = useTheme();

    const [loading, setLoading] = useState<boolean>(true);
    const [hasBeenSubmitted, SetHasBeenSubmitted] = useState<boolean>(false);

    const [selectedImage, setSelectedImage] = useState<File | undefined>(
        undefined
    );
    const [avatar, setAvatar] = useState<string | undefined>(
        customer?.avatarImage
            ? customer.avatarImage
            : getImageUrl(
                `avatar-${customer && customer !== null && customer?.avatar ? customer.avatar : 1}.png`,
                ImagePath.USERS
            )
    );
    const [trainingCentres, setTrainingCentres] = useState<TrainingCentre[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [ethnicities, setEthnicities] = useState<Ethnicity[]>([]);
    const [skills, setSkills] = useState<Skill[]>([]);
    const [avatarBase64, setAvatarBase64] = useState<string | null>(null);
    const [countries, setCountries] = useState<Country[]>([]);
    const [documents, setDocuments] = useState<File[]>([]);

    const { user, isLoggedIn } = useContext(DuendeContext);
    console.log("LEAD", customer)
    useEffect(() => {
        if (customer != null) {
            const files: File[] = customer.fileDocuments.map(
                (doc) => new File([], doc.name)
            );
            setDocuments(files);
        }
    }, [customer]);


    useEffect(() => {
        if (selectedImage) {
            setAvatar(URL.createObjectURL(selectedImage));
        }
    }, [selectedImage]);

    useEffect(() => {
        setLoading(false);
    }, []);

    useEffect(() => {
        const fetchTrainingCentres = async () => {
            try {
                const data: TrainingCentre[] = await getTrainingCentres();
                //const data1:TrainingCentre[] = [];
                setTrainingCentres(data);
                console.log("TRAINING CENTRES", data);
            } catch (error) {
                setError("Failed to fetch training centres");
                console.error(error);
            }
        };

        fetchTrainingCentres();
    }, []);

    useEffect(() => {
        async function fetchEthnicities() {
            try {
                const data = await getEthnicities();
                setEthnicities(data);
            } catch (error) {
                console.error("Error fetching ethnicities:", error);
            }
        }

        fetchEthnicities();
    }, []);

    useEffect(() => {
        const fetchSkills = async () => {
            const skillsData = await getSkills();
            setSkills(skillsData);
        };
        fetchSkills();
    }, []);

    useEffect(() => {
        const fetchCountries = async () => {
            try {
                const response = await getCountries();
                setCountries(response);
            } catch (error) {
                console.error("Error fetching countries:", error);
            }
        };

        fetchCountries();
    }, []);

    const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (file) {
            setSelectedImage(e.target.files?.[0]);
            const reader = new FileReader();
            reader.onloadend = () => {
                const fileBase64 = reader.result as string;
                setAvatarBase64(fileBase64);
            };
            reader.readAsDataURL(file);
        }
    };

    const handleFilesUpload = (files: File[]) => {
        console.log("Files received from child:", files);
        setDocuments(files);
        // Handle the files as needed
    };

    const handleFileRemove = (file: File) => {
        // Handle file removal logic
        console.log('File removed:', file);
        
        // Update the documents state if necessary
        setDocuments((prevDocs) => prevDocs.filter(doc => doc.name !== file.name));
      };
    
    const CustomerSchema = Yup.object().shape({
        firstName: Yup.string().max(255).required("First Name is required"),
        lastName: Yup.string().max(255).required("Last Name is required"),
        age: Yup.number().required("Age is required"),
        email: Yup.string()
            .max(255)
            .required("Email is required")
            .email("Must be a valid email"),
        status: Yup.string().required("Status is required"),
        location: Yup.string().max(500),
        about: Yup.string().max(500),
        gender: Yup.mixed().oneOf([Gender.MALE, Gender.FEMALE, Gender.NONBINARY, Gender.PREFERNOTTOSAY], 'Invalid gender').required('Gender is required'),
        country: Yup.string().max(255).required("Country is required"),
        dateOfBirth: Yup.date()
            .max(new Date(new Date().setFullYear(new Date().getFullYear() - 18)), "You must be at least 18 years old")
            .required("Date of birth is required"),
        ethnicity: Yup.string().max(255).required("Ethnicity of birth is required"),
        employeeStatus: Yup.string().max(255).required("Employee status is required"),
        city: Yup.string().max(255).required("City is required"),
        postcode: Yup.string().max(255).required("Postcode is required"),
        street: Yup.string().max(255).required("Street is required"),
        number: Yup.string().max(255).required("House number is required"),

    });

    const [openAlert, setOpenAlert] = useState(false);

    const handleAlertClose = () => {
        setOpenAlert(!openAlert);
        closeModal();
    };

    const formik = useFormik({
        initialValues: getInitialValues(customer!),
        validationSchema: CustomerSchema,
        enableReinitialize: true,
        onSubmit: async (values, { setSubmitting }) => {
            SetHasBeenSubmitted(true);

            try {
                let newCustomer: LeadListExtended = values as unknown as LeadListExtended;
                newCustomer.name = newCustomer.firstName + " " + newCustomer.lastName;
                newCustomer.avatarImage = avatarBase64;
                console.log("NEWLEAD",newCustomer);
                if (customer) {
                    setSubmitting(true); // Set submitting state to true
                    updateCustomerWithDocuments(
                        newCustomer.id!,
                        newCustomer,
                        documents
                    ).then((success) => {
                        if (success) {
                            openSnackbar({
                                open: true,
                                message: "Customer updated successfully.",
                                variant: "alert",
                                alert: {
                                    color: "success",
                                },
                            } as SnackbarProps);
                        } else {
                            openSnackbar({
                                open: true,
                                message: "Failed to update customer.",
                                variant: "alert",
                                alert: {
                                    color: "error",
                                },
                            } as SnackbarProps);
                        }
                        setSubmitting(false);
                        closeModal();
                    }).catch((error) => {
                        console.error('Error:', error);
                        setSubmitting(false);
                    });
                } else {
                    setSubmitting(true); // Set submitting state to true


                    if (user.trainingCentreId === null && newCustomer.trainingCentre === 0 && user.role ==="Super Admin")
                    {
                        newCustomer.trainingCentre = 0;
                    }
                    else if (user.role ==="Training Centre" || user.role ==="Candidate")
                        newCustomer.trainingCentre = user.trainingCentreId;


                    insertCustomerWithDocuments(
                        newCustomer,
                        documents
                    ).then((success) => {
                        if (success) {
                            openSnackbar({
                                open: true,
                                message: "Customer added successfully.",
                                variant: "alert",
                                alert: {
                                    color: "success",
                                },
                            } as SnackbarProps);
                        } else {
                            openSnackbar({
                                open: true,
                                message: "Failed to add customer.",
                                variant: "alert",
                                alert: {
                                    color: "error",
                                },
                            } as SnackbarProps);
                        }
                        setSubmitting(false);
                        closeModal();
                    }).catch((error) => {
                        console.error('Error:', error);
                        setSubmitting(false);
                    });
                }
            } catch (error) {
                console.error(error);
            }

            SetHasBeenSubmitted(false);

        },
    });
    const [value, setValue] = useState(0);
    const handleChange = (event, newValue) => { setValue(newValue); };
    const {
        errors,
        touched,
        handleSubmit,
        isSubmitting,
        getFieldProps,
        setFieldValue,
    } = formik;

    if (loading)
        return (
            <Box sx={{ p: 5 }}>
                <Stack direction="row" justifyContent="center">
                    <CircularWithPath />
                </Stack>
            </Box>
        );

    return (
        <Box sx={{ bgcolor: 'background.paper', p: 4 }}>
            
            {customer ? 
            <Tabs value={value} onChange={handleChange} aria-label="tabs">
                <Tab label="Candidate" />
                <Tab label="Qualifications" />
            </Tabs>:<></>}
            <Box sx={{ mt: 2 }}>
                {value === 0 && (
                    <>
                        <FormikProvider value={formik}>
                            <LocalizationProvider dateAdapter={AdapterDateFns}>
                                <Form autoComplete="off" noValidate onSubmit={handleSubmit}>
                                    <DialogTitle>
                                        {customer ? "Edit Lead" : "New Lead"}
                                    </DialogTitle>
                                    <Divider />
                                    <DialogContent sx={{ p: 2.5 }}>
                                        <Grid container spacing={3}>
                                            <Grid item xs={12} md={3}>
                                                <Stack direction="row" justifyContent="center" sx={{ mt: 3 }}>
                                                    <FormLabel
                                                        htmlFor="change-avtar"
                                                        sx={{
                                                            position: "relative",
                                                            borderRadius: "50%",
                                                            overflow: "hidden",
                                                            "&:hover .MuiBox-root": { opacity: 1 },
                                                            cursor: "pointer",
                                                        }}
                                                    >
                                                        <Avatar
                                                            alt="Avatar 1"
                                                            src={avatar}
                                                            sx={{ width: 72, height: 72, border: "1px dashed" }}
                                                        />
                                                        <Box
                                                            sx={{
                                                                position: "absolute",
                                                                top: 0,
                                                                left: 0,
                                                                background:
                                                                    theme.palette.mode === ThemeMode.DARK
                                                                        ? "rgba(255, 255, 255, .75)"
                                                                        : "rgba(0,0,0,.65)",
                                                                width: "100%",
                                                                height: "100%",
                                                                opacity: 0,
                                                                display: "flex",
                                                                alignItems: "center",
                                                                justifyContent: "center",
                                                            }}
                                                        >
                                                            <Stack spacing={0.5} alignItems="center">
                                                                <CameraOutlined
                                                                    style={{
                                                                        color: theme.palette.secondary.lighter,
                                                                        fontSize: "2rem",
                                                                    }}
                                                                />
                                                                <Typography sx={{ color: "secondary.lighter" }}>
                                                                    Upload
                                                                </Typography>
                                                            </Stack>
                                                        </Box>
                                                    </FormLabel>
                                                    <TextField
                                                        type="file"
                                                        id="change-avtar"
                                                        placeholder="Outlined"
                                                        variant="outlined"
                                                        sx={{ display: "none" }}
                                                        onChange={handleFileChange}
                                                    />
                                                </Stack>
                                            </Grid>
                                            <Grid item xs={12} md={8}>
                                                <Grid container spacing={3}>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-firstName">
                                                                First Name
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-firstName"
                                                                placeholder="Enter First Name"
                                                                {...getFieldProps("firstName")}
                                                                error={Boolean(touched.firstName && errors.firstName)}
                                                                helperText={touched.firstName && errors.firstName}
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-lastName">
                                                                Last Name
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-lastName"
                                                                placeholder="Enter Last Name"
                                                                {...getFieldProps("lastName")}
                                                                error={Boolean(touched.lastName && errors.lastName)}
                                                                helperText={touched.lastName && errors.lastName}
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-email">Email</InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-email"
                                                                placeholder="Enter Customer Email"
                                                                {...getFieldProps("email")}
                                                                error={Boolean(touched.email && errors.email)}
                                                                helperText={touched.email && errors.email}
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-appointmentDate">
                                                                Appointment Date
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-appointmentDate"
                                                                type="date"
                                                                {...getFieldProps("appointmentDate")}
                                                            />
                                                        </Stack>{" "}
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-appointmentTime">
                                                                Appointment Time
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-appointmentTime"
                                                                type="time"
                                                                {...getFieldProps("appointmentTime")}
                                                            />
                                                        </Stack>{" "}
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-dateOfBirth">
                                                                Date of Birth
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-dateOfBirth"
                                                                type="date"
                                                                error={Boolean(
                                                                    touched.dateOfBirth && errors.dateOfBirth
                                                                )}
                                                                {...getFieldProps("dateOfBirth")}
                                                            />
                                                        </Stack>{" "}
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-ethnicity">
                                                                Ethnicity
                                                            </InputLabel>
                                                            <Select
                                                                fullWidth
                                                                id="customer-ethnicity"
                                                                {...getFieldProps("ethnicity")}
                                                                error={Boolean(touched.ethnicity && errors.ethnicity)}
                                                            >
                                                                {ethnicities.map((ethnicity) => (
                                                                    <MenuItem key={ethnicity.id} value={ethnicity.name}>
                                                                        {ethnicity.name}
                                                                    </MenuItem>
                                                                ))}
                                                            </Select>
                                                        </Stack>
                                                    </Grid>

                                                    <Grid item xs={12}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-disability">
                                                                Disability
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-disability"
                                                                multiline
                                                                rows={2}
                                                                {...getFieldProps("disability")}
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-employeeStatus">
                                                                Employee Status
                                                            </InputLabel>
                                                            <Select
                                                                fullWidth
                                                                id="customer-employeeStatus"
                                                                {...getFieldProps("employeeStatus")}
                                                                error={Boolean(
                                                                    touched.employeeStatus && errors.employeeStatus
                                                                )}
                                                            >
                                                                {/* Add options for employee status */}
                                                                <MenuItem value="employed">Employed</MenuItem>
                                                                <MenuItem value="selfEmployed">
                                                                    Self-Employed
                                                                </MenuItem>
                                                                <MenuItem value="unemployed">Unemployed</MenuItem>
                                                            </Select>
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-employer">
                                                                Employer
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-employer"
                                                                {...getFieldProps("employer")}
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    {user && user.role === "Super Admin" ? (
    <Grid item xs={12}>
        <Stack spacing={1}>
            <InputLabel htmlFor="customer-trainingCentre">
                Training Centre
            </InputLabel>
            <Select
                fullWidth
                id="customer-trainingCentre"
                {...getFieldProps("trainingCentre")}
                error={Boolean(touched.trainingCentre && errors.trainingCentre)}
            >
                {trainingCentres.length > 0 ? (
                    trainingCentres.map((centre) => (
                        <MenuItem key={centre.id} value={centre.id}>
                            {centre.name}
                        </MenuItem>
                    ))
                ) : (
                    <MenuItem value="" disabled>
                        No training centres available
                    </MenuItem>
                )}
            </Select>
        </Stack>
    </Grid>
) : user && user.role === "Training Centre" ? (
    <Grid item xs={12}>
        <input type="hidden" id="customer-trainingCentre" value={user.trainingCentreId} {...getFieldProps("trainingCentre")} />
    </Grid>
) : (
    <></>
)}

                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-nationalInsurance">
                                                                UK National Insurance
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-nationalInsurance"
                                                                {...getFieldProps("nationalInsurance")}
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    {user && user.role === "Super Admin" ?
                                                        <Grid item xs={12} sm={6}>
                                                            <Stack spacing={1}>
                                                                <InputLabel htmlFor="customer-role">
                                                                    Customer Role
                                                                </InputLabel>
                                                                <Select
                                                                    fullWidth
                                                                    id="customer-role"
                                                                    {...getFieldProps("role")}
                                                                    error={Boolean(touched.role && errors.role)}
                                                                >
                                                                    <MenuItem value="Super Admin">Super Admin</MenuItem>
                                                                    <MenuItem value="Training Centre">
                                                                        Training Centre
                                                                    </MenuItem>
                                                                    <MenuItem value="Candidate">Candidate</MenuItem>
                                                                </Select>
                                                                {touched.role && errors.role && (
                                                                    <FormHelperText error>{errors.role}</FormHelperText>
                                                                )}
                                                            </Stack>
                                                        </Grid> : <></>}

                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-gender">
                                                                Gender
                                                            </InputLabel>
                                                            <RadioGroup
                                                                row
                                                                // error={Boolean(touched.gender && errors.gender)}
                                                                aria-label="payment-card"
                                                                {...getFieldProps("gender")}
                                                            >
                                                                <FormControlLabel
                                                                    control={<Radio value={Gender.FEMALE} />}
                                                                    label={Gender.FEMALE}
                                                                />
                                                                <FormControlLabel
                                                                    control={<Radio value={Gender.MALE} />}
                                                                    label={Gender.MALE}
                                                                />
                                                                <FormControlLabel
                                                                    control={<Radio value={Gender.NONBINARY} />}
                                                                    label={Gender.NONBINARY}
                                                                />
                                                                <FormControlLabel
                                                                    control={<Radio value={Gender.PREFERNOTTOSAY} />}
                                                                    label={Gender.PREFERNOTTOSAY}
                                                                />
                                                            </RadioGroup>
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-status">
                                                                Status
                                                            </InputLabel>
                                                            <FormControl fullWidth>
                                                                <Select
                                                                    id="column-hiding"
                                                                    displayEmpty
                                                                    {...getFieldProps("status")}
                                                                    onChange={(event: SelectChangeEvent<string>) =>
                                                                        setFieldValue(
                                                                            "status",
                                                                            event.target.value as string
                                                                        )
                                                                    }
                                                                    input={
                                                                        <OutlinedInput
                                                                            id="select-column-hiding"
                                                                            placeholder="Sort by"
                                                                        />
                                                                    }
                                                                    renderValue={(selected) => {
                                                                        if (!selected) {
                                                                            return (
                                                                                <Typography variant="subtitle1">
                                                                                    Select Status
                                                                                </Typography>
                                                                            );
                                                                        }

                                                                        const selectedStatus = allStatus.filter(
                                                                            (item) => item.value === Number(selected)
                                                                        );
                                                                        return (
                                                                            <Typography variant="subtitle2">
                                                                                {selectedStatus.length > 0
                                                                                    ? selectedStatus[0].label
                                                                                    : "Pending"}
                                                                            </Typography>
                                                                        );
                                                                    }}
                                                                >
                                                                    {allStatus.map((column: StatusProps) => (
                                                                        <MenuItem key={column.value} value={column.value}>
                                                                            <ListItemText primary={column.label} />
                                                                        </MenuItem>
                                                                    ))}
                                                                </Select>
                                                            </FormControl>
                                                            {touched.status && errors.status && (
                                                                <FormHelperText
                                                                    error
                                                                    id="standard-weight-helper-text-email-login"
                                                                    sx={{ pl: 1.75 }}
                                                                >
                                                                    {errors.status}
                                                                </FormHelperText>
                                                            )}
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12} sm={12}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-contact">
                                                                Contact
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-contact"
                                                                placeholder="Enter Contact"
                                                                {...getFieldProps("contact")}
                                                                error={Boolean(touched.contact && errors.contact)}
                                                                helperText={touched.contact && errors.contact}
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-number">
                                                                House Number
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-number"
                                                                placeholder="Enter Number"
                                                                {...getFieldProps("number")}
                                                                error={Boolean(touched.number && errors.number)}
                                                                helperText={touched.number && errors.number}
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-street">
                                                                Street
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-street"
                                                                placeholder="Enter Street"
                                                                {...getFieldProps("street")}
                                                                error={Boolean(touched.street && errors.street)}
                                                                helperText={touched.street && errors.street}
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-city">City</InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-city"
                                                                placeholder="Enter City"
                                                                {...getFieldProps("city")}
                                                                error={Boolean(touched.city && errors.city)}
                                                                helperText={touched.city && errors.city}
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-postcode">
                                                                Postcode
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-postcode"
                                                                placeholder="Enter Postcode"
                                                                {...getFieldProps("postcode")}
                                                                error={Boolean(touched.postcode && errors.postcode)}
                                                                helperText={touched.postcode && errors.postcode}
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-country">
                                                                Country
                                                            </InputLabel>
                                                            <FormControl
                                                                fullWidth
                                                                error={Boolean(
                                                                    formik.touched.country && formik.errors.country
                                                                )}
                                                            >
                                                                {countries.length > 0 ? (
                                                                    <Select
                                                                        id="customer-country"
                                                                        value={formik.values.country}
                                                                        onChange={formik.handleChange}
                                                                        onBlur={formik.handleBlur}
                                                                        name="country"
                                                                        displayEmpty
                                                                    >
                                                                        <MenuItem value="" disabled>
                                                                            Select Country
                                                                        </MenuItem>
                                                                        {countries.map((country) => (
                                                                            <MenuItem key={country.id} value={country.name}>
                                                                                {country.name}
                                                                            </MenuItem>
                                                                        ))}
                                                                    </Select>
                                                                ) : (
                                                                    <>Loading...</>
                                                                )}
                                                                {formik.touched.country && formik.errors.country && (
                                                                    <FormHelperText>
                                                                        {formik.errors.country}
                                                                    </FormHelperText>
                                                                )}
                                                            </FormControl>
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-location">
                                                                Location
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-location"
                                                                multiline
                                                                rows={2}
                                                                placeholder="Enter Location"
                                                                {...getFieldProps("location")}
                                                                helperText={touched.location && errors.location}
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-about">
                                                                About Customer
                                                            </InputLabel>
                                                            <TextField
                                                                fullWidth
                                                                id="customer-about"
                                                                multiline
                                                                rows={2}
                                                                placeholder="Enter Customer Information"
                                                                {...getFieldProps("about")}
                                                                helperText={touched.about && errors.about}
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-skills">
                                                                Skills
                                                            </InputLabel>
                                                            <Autocomplete
                                                                multiple
                                                                fullWidth
                                                                id="customer-skills"
                                                                options={skills.map((skill) => skill.name)}
                                                                {...getFieldProps("skills")}
                                                                getOptionLabel={(label) => label}
                                                                onChange={(event, newValue) => {
                                                                    setFieldValue("skills", newValue);
                                                                }}
                                                                renderInput={(params) => (
                                                                    <TextField
                                                                        {...params}
                                                                        name="skill"
                                                                        placeholder="Add Skills"
                                                                    />
                                                                )}
                                                                renderTags={(value, getTagProps) =>
                                                                    value.map((option, index) => (
                                                                        <Chip
                                                                            {...getTagProps({ index })}
                                                                            variant="combined"
                                                                            key={index}
                                                                            label={option}
                                                                            deleteIcon={
                                                                                <CloseOutlined
                                                                                    style={{ fontSize: "0.75rem" }}
                                                                                />
                                                                            }
                                                                            sx={{ color: "text.primary" }}
                                                                        />
                                                                    ))
                                                                }
                                                            />
                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-documents">
                                                                Documents
                                                            </InputLabel>

                                                                  <MultipleFileUploader
        onFilesUpload={handleFilesUpload}
        onFileRemove={handleFileRemove} // Add this line
                                                                        // @ts-ignore

        detailedFiles={documents}
        initialFiles={documents}
        acceptedFiles="image/*"
      />

                                                        </Stack>
                                                    </Grid>
                                                    <Grid item xs={12} sm={6}>
                                                        <Stack spacing={1}>
                                                            <InputLabel htmlFor="customer-portfolio">
                                                                Portfolio
                                                            </InputLabel>
                                                            <Select
                                                                fullWidth
                                                                id="customer-portfolio"
                                                                {...getFieldProps("portfolio")}
                                                            >
                                                                <MenuItem value="original">Original</MenuItem>
                                                                <MenuItem value="renewal">Renewal</MenuItem>
                                                            </Select>
                                                        </Stack>
                                                    </Grid>
                                                </Grid>
                                            </Grid>
                                        </Grid>
                                    </DialogContent>
                                    <Divider />
                                    <DialogActions sx={{ p: 2.5 }}>
                                        <Grid
                                            container
                                            justifyContent="space-between"
                                            alignItems="center"
                                        >
                                            <Grid item>
                                                {customer && (
                                                    <Tooltip title="Delete Customer" placement="top">
                                                        <IconButton
                                                            onClick={() => setOpenAlert(true)}
                                                            size="large"
                                                            color="error"
                                                        >
                                                            <DeleteFilled />
                                                        </IconButton>
                                                    </Tooltip>
                                                )}
                                            </Grid>
                                            <Grid item>
                                                <Stack direction="row" spacing={2} alignItems="center">
                                                    <Button color="error" onClick={closeModal}>
                                                        Cancel
                                                    </Button>
                                                    {hasBeenSubmitted === false ? (
                                                        <Button
                                                            type="submit"
                                                            variant="contained"
                                                            disabled={isSubmitting}
                                                        >
                                                            {customer ? "Save" : "Add"}
                                                        </Button>
                                                    ) : (
                                                        <CircularProgress size={44} />
                                                    )}
                                                </Stack>
                                            </Grid>
                                        </Grid>
                                    </DialogActions>
                                    <Divider />
                                </Form>
                            </LocalizationProvider>
                        </FormikProvider>
                        {customer && (
                            <AlertCustomerDelete
                                id={customer.id!}
                                title={customer.name}
                                open={openAlert}
                                handleClose={handleAlertClose}
                            />
                        )}
                    </>
                )}
                {user && (user.role === "Training Centre" || user.role === "Super Admin") && value === 1 && (
                    <>
                        <DynamicTableCustomerWithCourseQualificationRecords customerId={customer.id}/>
                    </>
                )}
            </Box>
        </Box>
    );
}

