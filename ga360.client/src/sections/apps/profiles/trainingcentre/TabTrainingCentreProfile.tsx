// material-ui
import { Theme } from "@mui/material/styles";
import useMediaQuery from "@mui/material/useMediaQuery";
import Chip from "@mui/material/Chip";
import Divider from "@mui/material/Divider";
import Grid from "@mui/material/Grid";
import Link from "@mui/material/Link";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemSecondaryAction from "@mui/material/ListItemSecondaryAction";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";

// third-party
import { PatternFormat } from "react-number-format";

// project import
import MainCard from "components/MainCard";
import Avatar from "components/@extended/Avatar";
import LinearWithLabel from "components/@extended/progress/LinearWithLabel";

// assets
import AimOutlined from "@ant-design/icons/AimOutlined";
import EnvironmentOutlined from "@ant-design/icons/EnvironmentOutlined";
import MailOutlined from "@ant-design/icons/MailOutlined";
import PhoneOutlined from "@ant-design/icons/PhoneOutlined";

import defaultImages from "assets/images/users/default.png";
import { CertificationViewDataProps, TableDataProps } from "types/table";
import makeData from "data/react-table";
import { useContext, useEffect, useMemo, useState } from "react";
import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  getPaginationRowModel,
  getSortedRowModel,
  HeaderGroup,
  SortingState,
  useReactTable,
} from "@tanstack/react-table";
import ReactTable from "data/react-table";
import {
  Box,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableFooter,
  TableHead,
  TableRow,
} from "@mui/material";
import {
  CSVExport,
  HeaderSort,
  SelectColumnSorting,
  TablePagination,
} from "components/third-party/react-table";
import ScrollX from "components/ScrollX";

// ==============================|| ACCOUNT PROFILE - BASIC ||============================== //
// types
import { LabelKeyObject } from "react-csv/lib/core";
import { getCandidate } from "api/customer";
import { CustomerListExtended } from "types/customer";
import { getImageUrl, ImagePath } from "utils/getImageUrl";
import CourseProgressions from "./CourseTrainingCentreProfile";
import { CertificateModel } from "types/customerApiModel";
import chartsMap from "menu-items/charts-map";
import TrainingCentreProfile from "./TrainingCentreProfile";
import DuendeContext from "contexts/DuendeContext";
import { getTrainingCentre, TrainingCentre } from "api/trainingcentreService";

export default function TabTrainingCentreProfile() {
  const { user, isLoggedIn } = useContext(DuendeContext);

  const [candidate, setCandidate] = useState<CustomerListExtended>(null);
  const [trainingCentre, setTrainingCentre] = useState<TrainingCentre>(null);

  const [avatar, setAvatar] = useState<string | undefined>(
    candidate?.avatarImage ? candidate.avatarImage : defaultImages
  );

  const matchDownMD = useMediaQuery((theme: Theme) =>
    theme.breakpoints.down("md")
  );
  const columns = useMemo<ColumnDef<CertificationViewDataProps>[]>(
    () => [
      {
        header: "Card/cert name",
        footer: "Card/cert name",
        accessorKey: "name",
      },
      {
        header: "Type",
        footer: "Type",
        accessorKey: "type",
      },
      {
        header: "Date/Time",
        footer: "Date/Time",
        accessorKey: "regDate",
        meta: {
          className: "cell-right",
        },
      },
      {
        header: "Charge",
        footer: "Charge",
        accessorKey: "charge",
      },
    ],
    []
  );

  const mapCertificates = (
    certificatesModel: CertificateModel[]
  ): CertificationViewDataProps[] => {
    return certificatesModel.map((certificatemodel) => ({
      name: certificatemodel.name,
      // id:certificate.id,
      charge: certificatemodel.charge,
      type: certificatemodel.type,
      regDate: new Date(certificatemodel.date).toLocaleDateString("en-GB"),
    }));
  };

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await getCandidate();
        const trainingCentreResponse = await getTrainingCentre(response.trainingCentre);
        setAvatar(response.avatarImage);
        setCandidate(response);
        setTrainingCentre(trainingCentreResponse);
      } catch (error) {
        console.error("Error fetching profile:", error);
      }
    };

    fetchUser();
  }, []);

  useEffect(() => {
    const fetchTrainingCentre = async () => {
      try {
        if (candidate && candidate.trainingCentre !== null) {
          const response = await getTrainingCentre(candidate.trainingCentre);
          setTrainingCentre(response);
        }
      } catch (error) {
        console.error("Error fetching training centre:", error);
      }
    };

    fetchTrainingCentre();
  }, []);

  return (
    <Grid container spacing={3}>
      <Grid item xs={12} sm={5} md={4} xl={3}>
        <Grid container spacing={3}>
          <Grid item xs={12}>
            <TrainingCentreProfile />
          </Grid>
          {/* <Grid item xs={12}>
                        <CourseProgressions />
                    </Grid> */}
        </Grid>
      </Grid>
      <Grid item xs={12} sm={7} md={8} xl={9}>
        <Grid container spacing={3}>
          <Grid item xs={12}>
            <MainCard title="About me">
              <Typography color="secondary">
                {candidate && candidate.about}
              </Typography>
            </MainCard>
          </Grid>
          <Grid item xs={12}>
            <MainCard title="Personal Details">
              <List sx={{ py: 0 }}>
                <ListItem divider={!matchDownMD}>
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Full Name</Typography>
                        <Typography>
                          {candidate &&
                            candidate.firstName + " " + candidate.lastName}
                        </Typography>
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">NI</Typography>
                        <Typography>
                          {candidate && candidate.nationalInsurance}
                        </Typography>
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
                <ListItem divider={!matchDownMD}>
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Phone</Typography>
                        <Typography>
                          {candidate && candidate.contact}
                        </Typography>
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Country</Typography>
                        <Typography>
                          {candidate && candidate.country}
                        </Typography>
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
                <ListItem divider={!matchDownMD}>
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">DOB</Typography>
                        <Typography>
                          {candidate &&
                            candidate.dob &&
                            new Date(candidate.dob).toLocaleDateString("en-GB")}
                        </Typography>
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Gender</Typography>
                        <Typography>{candidate && candidate.gender}</Typography>
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
                <ListItem divider={!matchDownMD}>
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Phone</Typography>
                        <Typography>
                          {candidate && candidate.contact}
                        </Typography>
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Country</Typography>
                        <Typography>
                          {candidate && candidate.country}
                        </Typography>
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
                <ListItem divider={!matchDownMD}>
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Email</Typography>
                        <Typography>{candidate && candidate.email}</Typography>
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Postcode</Typography>
                        <Typography>
                          {candidate && candidate.postcode}
                        </Typography>
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
                <ListItem>
                  <Stack spacing={0.5}>
                    <Typography color="secondary">Address</Typography>
                    <Typography>
                      {candidate &&
                        `${candidate.number ? candidate.number + ", " : ""}${candidate.street ? candidate.street + ", " : ""}${candidate.city || ""}`}
                    </Typography>
                  </Stack>
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
                        <Typography>
                          {candidate && candidate.employmentStatus}
                        </Typography>
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Employer</Typography>
                        <Typography>
                          {candidate && candidate.employer}
                        </Typography>
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
              </List>
            </MainCard>
          </Grid>
          <Grid item xs={12}>
            <MainCard title="Training Centre Details">
              <List sx={{ py: 0 }}>
                <ListItem>
                  <Grid container spacing={matchDownMD ? 0.5 : 3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">
                          Training Centre
                        </Typography>
                        <Typography>
                          {trainingCentre && trainingCentre.name}
                        </Typography>
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Code Ref.</Typography>
                        <Typography>
                          {trainingCentre && trainingCentre.id}
                        </Typography>
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
                <ListItem>
                  <Grid container spacing={matchDownMD ? 0.5 : 3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Address</Typography>
                        <Typography>
                          <Typography>
                            {trainingCentre &&
                              trainingCentre.address &&
                              [
                                trainingCentre.address.number,
                                trainingCentre.address.street,
                                trainingCentre.address.postcode,
                              ]
                                .filter(Boolean)
                                .join(", ")}
                          </Typography>
                        </Typography>
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">City</Typography>
                        <Typography>
                          {trainingCentre &&
                            trainingCentre.address &&
                            trainingCentre.address.city}
                        </Typography>
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
