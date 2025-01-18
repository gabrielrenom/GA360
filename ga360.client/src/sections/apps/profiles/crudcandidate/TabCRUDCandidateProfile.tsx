import React, { useEffect, useState } from "react";
import { Theme } from "@mui/material/styles";
import useMediaQuery from "@mui/material/useMediaQuery";
import Grid from "@mui/material/Grid";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import MainCard from "components/MainCard";
import defaultImages from "assets/images/users/default.png";
import { CustomerListExtended } from "types/customer";
import CandidateProfile from "./CandidateCRUDProfile";
import MyQualificationsProfile from "./MyQualificationsCRUDProfile";
import { getUserById, updateCustomerWithDocuments } from "api/customer";
import { useParams } from "react-router";
import { getCountries } from "api/countryService";
import { Country } from "types/country";
import { MenuItem } from "@mui/material";
import { DatePicker, LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";

export default function TabCRUDCandidateProfile() {
  const { id } = useParams();
  const [candidate, setCandidate] = useState<CustomerListExtended>(null);
  const [avatar, setAvatar] = useState<string | undefined>(
    candidate?.avatarImage ? candidate.avatarImage : defaultImages
  );
  const matchDownMD = useMediaQuery((theme: Theme) =>
    theme.breakpoints.down("md")
  );
  const [editingField, setEditingField] = useState<string | null>(null);
  const [countries, setCountries] = useState<Country[]>([]);
  const [showCountryDropdown, setShowCountryDropdown] =
    useState<boolean>(false);

  const genderOptions = [
    { value: "Female", label: "Female" },
    { value: "Nonbinary", label: "Non Binary" },
    { value: "Male", label: "Male" },
    { value: "Prefer not to say", label: "Prefer not to say" },
  ];

  const employmentStatusOptions = [
    { value: "employed", label: "Employed" },
    { value: "selfEmployed", label: "Self Employed" },
    { value: "unemployed", label: "Unemployed" },
  ];

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await getUserById(Number(id));
        setAvatar(response.avatarImage);
        setCandidate(response);
      } catch (error) {
        console.error("Error fetching profile:", error);
      }
    };
    fetchUser();
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

  const handleFieldClick = (field: string) => {
    setEditingField(field);
  };

  const handleFieldChange = async (
    e: React.ChangeEvent<HTMLInputElement>,
    field: string
  ) => {
    console.log("FIELD", field);
    const learnerModel = { ...candidate, [field]: e.target.value };
    setCandidate(learnerModel);
    const files: File[] = learnerModel.fileDocuments.map(
      (doc) => new File([], doc.name)
    );
    await updateCustomerWithDocuments(learnerModel.id, learnerModel, files);
  };

  const handleFieldBlur = async (field: string) => {
    setEditingField(null);
  };

const renderField = (field: string, value: string | undefined, multiline: boolean = false, isDropdown: boolean = false, options: { value: string, label: string }[] = []) => {
  const todayMinus18Years = new Date();
  todayMinus18Years.setFullYear(todayMinus18Years.getFullYear() - 18);

  if (editingField === field) {
    if (field === 'dob') {
      return (
        <LocalizationProvider dateAdapter={AdapterDateFns} >
          <DatePicker
            value={value ? new Date(value) : null}
            onChange={(newValue) => {
              // @ts-ignore
              handleFieldChange({ target: { value: newValue } }, field);
            }}
            // @ts-ignore
            onBlur={() => handleFieldBlur(field)}
            renderInput={(params) => <TextField {...params} size="small" fullWidth />}
            disableFuture
            maxDate={todayMinus18Years}
            format="dd/MM/yyyy"
          />
        </LocalizationProvider>
      );
    } else if (isDropdown) {
      return (
        <TextField
          select
          variant="outlined"
          size="small"
          value={value || ""}
          // @ts-ignore
          onChange={(e) => handleFieldChange(e, field)}
          onBlur={() => handleFieldBlur(field)}
          autoFocus
          fullWidth
          sx={{ width: '200px' }} // Set the width as desired
        >
          {options.map((option) => (
            <MenuItem key={option.value} value={option.value}>
              {option.label}
            </MenuItem>
          ))}
        </TextField>
      );
    } else {
      return (
        <TextField
          variant="outlined"
          size="small"
          value={value || ""}
          // @ts-ignore
          onChange={(e) => handleFieldChange(e, field)}
          onBlur={() => handleFieldBlur(field)}
          autoFocus
          multiline={multiline}
          fullWidth={multiline}
          rows={multiline ? 4 : 1}
        />
      );
    }
  }

  // Find the corresponding label for the stored value
  const formattedValue = isDropdown && options ? options.find(option => option.value === value)?.label : value;

  return (
    <Typography
      onClick={() => handleFieldClick(field)}
      onDoubleClick={() => { if (field === "country") setShowCountryDropdown(true); }}
      style={{ cursor: "pointer" }}
    >
      {formattedValue}
    </Typography>
  );
};

// Usage
<Stack spacing={0.5}>
  <Typography color="secondary">
    Employment Status
  </Typography>
  {renderField(
    "employmentStatus",
    candidate?.employmentStatus,
    false,
    true,
    employmentStatusOptions
  )}
</Stack>


  return (
    <Grid container spacing={3}>
      <Grid item xs={12} sm={5} md={4} xl={3}>
        <Grid container spacing={3}>
          <Grid item xs={12}>
            <CandidateProfile candidate={candidate} defaultImages={avatar} />
          </Grid>
          <Grid item xs={12}>
            <MyQualificationsProfile userId={Number(id)} />
          </Grid>
        </Grid>
      </Grid>
      <Grid item xs={12} sm={7} md={8} xl={9}>
        <Grid container spacing={3}>
          <Grid item xs={12}>
            <MainCard title="About me">
              {renderField("about", candidate?.about, true)}
            </MainCard>
          </Grid>
          <Grid item xs={12}>
            <MainCard title="Personal Details">
              <List sx={{ py: 0 }}>
                <ListItem divider={!matchDownMD}>
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">First Name</Typography>
                        {renderField("firstName", candidate?.firstName)}
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Last Name</Typography>
                        {renderField("lastName", candidate?.lastName)}
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>

                <ListItem divider={!matchDownMD}>
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Phone</Typography>
                        {renderField("contact", candidate?.contact)}
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Country</Typography>
                        {renderField(
                          "country",
                          candidate?.country,
                          false,
                          true,
                          countries.map((country) => ({
                            value: country.name,
                            label: country.name,
                          }))
                        )}{" "}
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
                <ListItem divider={!matchDownMD}>
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">DOB</Typography>
                        {renderField(
                          "dob",
                          candidate?.dob &&
                            new Date(candidate.dob).toLocaleDateString("en-GB")
                        )}
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Gender</Typography>
                        {renderField(
                          "gender",
                          candidate?.gender,
                          false,
                          true,
                          genderOptions
                        )}
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
                <ListItem divider={!matchDownMD}>
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Email</Typography>
                        {renderField("email", candidate?.email)}
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Postcode</Typography>
                        {renderField("postcode", candidate?.postcode)}
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
                <ListItem divider={!matchDownMD}>
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Number</Typography>
                        {renderField("number", `${candidate?.number}`)}
                      </Stack>
                    </Grid>

                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Street</Typography>
                        {renderField("street", `${candidate?.street}`)}
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
                <ListItem>
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">City</Typography>
                        {renderField("city", `${candidate?.city}`)}
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">NI</Typography>
                        {renderField(
                          "nationalInsurance",
                          candidate?.nationalInsurance
                        )}
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
              </List>
            </MainCard>
          </Grid>
          <Grid item xs={12}>
            <MainCard title="Employment">
              <List sx={{ py: 0 }}>
                <ListItem>
                  <Grid container spacing={matchDownMD ? 0.5 : 3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">
                          Employment Status
                        </Typography>
                        {renderField(
                          "employeeStatus",
                          candidate?.employeeStatus,
                          false,
                          true,
                          employmentStatusOptions
                        )}
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Employer</Typography>
                        {renderField("employer", candidate?.employer)}
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
              </List>
            </MainCard>
          </Grid>
        </Grid>
      </Grid>
    </Grid>
  );
}
