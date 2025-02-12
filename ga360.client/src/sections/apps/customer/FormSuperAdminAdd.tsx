import { useEffect, useState, ChangeEvent, useContext } from "react";

// material-ui
import { useTheme } from "@mui/material/styles";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import CircularProgress from "@mui/material/CircularProgress";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import Divider from "@mui/material/Divider";
import FormControl from "@mui/material/FormControl";
import FormLabel from "@mui/material/FormLabel";
import Grid from "@mui/material/Grid";
import FormHelperText from "@mui/material/FormHelperText";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import Select, { SelectChangeEvent } from "@mui/material/Select";
import Stack from "@mui/material/Stack";
import TextField from "@mui/material/TextField";
import Tooltip from "@mui/material/Tooltip";
import Typography from "@mui/material/Typography";

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
  insertCustomerWithDocuments,
  updateCustomerWithDocuments,
} from "api/customer";
import { getImageUrl, ImagePath } from "utils/getImageUrl";

// assets
import CameraOutlined from "@ant-design/icons/CameraOutlined";
import DeleteFilled from "@ant-design/icons/DeleteFilled";

// types
import { SnackbarProps } from "types/snackbar";
import { CustomerList, CustomerListExtended } from "types/customer";
// import { TrainingCentre } from "types/trainingcentretypes";
import { getTrainingCentres, TrainingCentre } from "api/trainingcentreService";
import { Country } from "types/country";
import MultipleFileUploader from "components/MultipleFileUploader";
import { DocumentFileModel } from "types/customerApiModel";
import DuendeContext from "contexts/DuendeContext";
import {
  Tab,
  Tabs,
} from "@mui/material";

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
    trainingCentreId: 0,
  };

  if (customer) {
    return _.merge({}, newCustomer, customer);
  }

  return newCustomer;
};

// ==============================|| CUSTOMER ADD / EDIT - FORM ||============================== //

export default function FormSuperAdminAdd({
  customer,
  closeModal,
}: {
  customer: CustomerListExtended | null;
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
  const [avatarBase64, setAvatarBase64] = useState<string | null>(null);
  const [documents, setDocuments] = useState<File[]>([]);

  const { user } = useContext(DuendeContext);
  console.log(user);
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
        setTrainingCentres(data);
      } catch (error) {
        setError("Failed to fetch training centres");
        console.error(error);
      }
    };

    fetchTrainingCentres();
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
  };

  const handleFileRemove = (file: File) => {
    setDocuments((prevDocs) =>
      prevDocs.filter((doc) => doc.name !== file.name)
    );
  };

  const CustomerSchema = Yup.object().shape({
    firstName: Yup.string().max(255).required("First Name is required"),
    lastName: Yup.string().max(255).required("Last Name is required"),
    email: Yup.string()
      .max(255)
      .required("Email is required")
      .email("Must be a valid email"),
    gender: Yup.mixed()
      .oneOf(
        [Gender.MALE, Gender.FEMALE, Gender.NONBINARY, Gender.PREFERNOTTOSAY],
        "Invalid gender"
      )
      .required("Gender is required"),
    // trainingCentre: Yup.string().max(255).required("Training centre is required"),
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
        let newCustomer: CustomerListExtended = values as CustomerListExtended;
        newCustomer.name = newCustomer.firstName + " " + newCustomer.lastName;
        newCustomer.avatarImage = avatarBase64;
        if (customer) {
          setSubmitting(true); // Set submitting state to true
          updateCustomerWithDocuments(newCustomer.id!, newCustomer, documents)
            .then((success) => {
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
            })
            .catch((error) => {
              console.error("Error:", error);
              setSubmitting(false);
            });
        } else {
          setSubmitting(true); // Set submitting state to true

          if (
            user.trainingCentreId === null &&
            newCustomer.trainingCentre === 0 &&
            user.role === "Super Admin"
          ) {
            newCustomer.trainingCentre = 0;
          } else if (
            user.role === "Training Centre" ||
            user.role === "Candidate"
          )
            newCustomer.trainingCentre = user.trainingCentreId;

          insertCustomerWithDocuments(newCustomer, documents)
            .then((success) => {
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
            })
            .catch((error) => {
              console.error("Error:", error);
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
  const handleChange = (event, newValue) => {
    setValue(newValue);
  };
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
    <Box sx={{ bgcolor: "background.paper", p: 4 }}>
      {customer ? (
        <Tabs value={value} onChange={handleChange} aria-label="tabs">
          <Tab label="Administrator" />
        </Tabs>
      ) : (
        <></>
      )}
      <Box sx={{ mt: 2 }}>
        {value === 0 && (
          <>
            <FormikProvider value={formik}>
              <LocalizationProvider dateAdapter={AdapterDateFns}>
                <Form autoComplete="off" noValidate onSubmit={handleSubmit}>
                  <DialogTitle>
                    {customer ? "Edit Administrator" : "New Administrator"}
                  </DialogTitle>
                  <Divider />
                  <DialogContent sx={{ p: 2.5 }}>
                    <Grid container spacing={3}>
                      <Grid item xs={12} md={3}>
                        <Stack
                          direction="row"
                          justifyContent="center"
                          sx={{ mt: 3 }}
                        >
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
                              sx={{
                                width: 72,
                                height: 72,
                                border: "1px dashed",
                              }}
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
                                error={Boolean(
                                  touched.firstName && errors.firstName
                                )}
                                helperText={
                                  touched.firstName && errors.firstName
                                }
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
                                error={Boolean(
                                  touched.lastName && errors.lastName
                                )}
                                helperText={touched.lastName && errors.lastName}
                              />
                            </Stack>
                          </Grid>
                          <Grid item xs={12}>
                            <Stack spacing={1}>
                              <InputLabel htmlFor="customer-email">
                                Email
                              </InputLabel>
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
                                  error={Boolean(
                                    touched.trainingCentre &&
                                      errors.trainingCentre
                                  )}
                                >
                                  {trainingCentres.length > 0 ? (
                                    trainingCentres.map((centre) => (
                                      <MenuItem
                                        key={centre.id}
                                        value={centre.id}
                                      >
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
                              <input
                                type="hidden"
                                id="customer-trainingCentre"
                                value={user.trainingCentreId}
                                {...getFieldProps("trainingCentre")}
                              />
                            </Grid>
                          ) : (
                            <></>
                          )}
                          {user && user.role === "Super Admin" ? (
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
                                  <MenuItem value="Super Admin">
                                    Super Admin
                                  </MenuItem>
                                  <MenuItem value="Training Centre">
                                    Training Centre
                                  </MenuItem>
                                </Select>
                                {touched.role && errors.role && (
                                  <FormHelperText error>
                                    {errors.role}
                                  </FormHelperText>
                                )}
                              </Stack>
                            </Grid>
                          ) : (
                            <></>
                          )}

<Grid item xs={12} sm={6}>
        <Stack spacing={1}>
          <InputLabel htmlFor="customer-gender">Gender</InputLabel>
          <FormControl fullWidth>
            <Select
              labelId="customer-gender"
              id="customer-gender"
              defaultValue=""
              displayEmpty
              {...getFieldProps("gender")}
            >
              <MenuItem value="" disabled>
                Please select gender
              </MenuItem>
              <MenuItem value={Gender.MALE}>{Gender.MALE}</MenuItem>
              <MenuItem value={Gender.FEMALE}>{Gender.FEMALE}</MenuItem>
              <MenuItem value={Gender.NONBINARY}>{Gender.NONBINARY}</MenuItem>
              <MenuItem value={Gender.PREFERNOTTOSAY}>{Gender.PREFERNOTTOSAY}</MenuItem>
            </Select>
          </FormControl>
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
                                error={Boolean(
                                  touched.contact && errors.contact
                                )}
                                helperText={touched.contact && errors.contact}
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
                              {customer
                                ? "Save Administrator"
                                : "Create Administrator"}
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
      </Box>
    </Box>
  );
}
