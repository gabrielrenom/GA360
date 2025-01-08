import React, { useState, useEffect } from "react";
import {
  useMediaQuery,
  Grid,
  Chip,
  Divider,
  Link,
  List,
  ListItem,
  ListItemIcon,
  ListItemSecondaryAction,
  ListItemText,
  Stack,
  Typography,
  Theme,
} from "@mui/material";
import { PatternFormat } from "react-number-format";
import MainCard from "components/MainCard";
import Avatar from "components/@extended/Avatar";
import { getImageUrl, ImagePath } from "utils/getImageUrl";
import EnvironmentOutlined from "@ant-design/icons/EnvironmentOutlined";
import MailOutlined from "@ant-design/icons/MailOutlined";
import PhoneOutlined from "@ant-design/icons/PhoneOutlined";
import {
  CustomerProfileModel,
  getCustomerProfileHighPerformance,
} from "api/customer";

interface StatusProps {
  value: number;
  label: string;
}

const allStatus: StatusProps[] = [
  { value: 3, label: "Rejected" },
  { value: 1, label: "Verified" },
  { value: 2, label: "Pending" },
];

export default function ExpandingUserDetail({ data }: any) {
  const downMD = useMediaQuery((theme: Theme) => theme.breakpoints.down("md"));
  const selectedStatus = allStatus.filter(
    (item) => item.value === Number(data.status)
  );

  const [profile, setProfile] = useState<CustomerProfileModel | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const result = await getCustomerProfileHighPerformance(data.id);
        console.log("RESULT", result);
        setProfile(result);
      } catch (error) {
        setError("Failed to fetch customer profile");
      }
    };
    fetchProfile();
  }, [data.id]);

  if (error) {
    return <div>{error}</div>;
  }

  if (!profile) {
    return <div>Loading...</div>;
  }

  return (
    <Grid
      container
      spacing={2.5}
      sx={{ pl: { xs: 0, sm: 5, md: 6, lg: 10, xl: 12 } }}
    >
      <Grid item xs={12} sm={5} md={4} xl={3.5}>
        {profile && (
          <MainCard>
            <Chip
              label={
                selectedStatus.length > 0 ? selectedStatus[0].label : "Pending"
              }
              size="small"
              sx={{
                position: "absolute",
                right: -1,
                top: -1,
                borderRadius: "0 4px 0 4px",
              }}
            />
            <Grid container spacing={3}>
              <Grid item xs={12}>
                <Stack spacing={2.5} alignItems="center">
                  <Avatar
                    alt="Avatar 1"
                    size="xl"
                    src={
                      profile.avatarImage && profile.avatarImage.trim()
                        ? profile.avatarImage
                        : getImageUrl("default-avatar.png", ImagePath.USERS)
                    }
                  />

                  <Stack spacing={0.5} alignItems="center">
                    <Typography variant="h5">
                      {profile.firstName} {profile.lastName}
                    </Typography>
                  </Stack>
                </Stack>
              </Grid>
              <Grid item xs={12}>
                <Divider />
              </Grid>
              <Grid item xs={12}>
                <Stack
                  direction="row"
                  justifyContent="space-around"
                  alignItems="center"
                >
                  <Stack spacing={0.5} alignItems="center">
                    <Typography variant="h5">
                      {new Date(profile.dob).toLocaleDateString("en-GB")}
                    </Typography>
                    <Typography color="secondary">Date of Birth</Typography>
                  </Stack>
                  <Divider orientation="vertical" flexItem />
                  <Stack spacing={0.5} alignItems="center">
                    <Typography variant="h5">
                      {profile.avgQualificationProgression}%
                    </Typography>
                    <Typography color="secondary">Progress</Typography>
                  </Stack>
                  <Divider orientation="vertical" flexItem />
                  <Stack spacing={0.5} alignItems="center">
                    <Typography variant="h5">
                      <Typography variant="h5">
                        {profile.employmentStatus === "employed"
                          ? "Employed"
                          : profile.employmentStatus === "selfEmployed"
                            ? "Self-Employed"
                            : profile.employmentStatus === "unemployed"
                              ? "Unemployed"
                              : "Unknown"}
                      </Typography>
                    </Typography>
                    <Typography color="secondary">Employment Status</Typography>
                  </Stack>
                </Stack>
              </Grid>
              <Grid item xs={12}>
                <Divider />
              </Grid>
              <Grid item xs={12}>
                <List
                  component="nav"
                  aria-label="main mailbox folders"
                  sx={{ py: 0, "& .MuiListItem-root": { p: 0 } }}
                >
                  <ListItem>
                    <ListItemIcon>
                      <MailOutlined />
                    </ListItemIcon>
                    <ListItemText
                      primary={<Typography color="secondary">Email</Typography>}
                    />
                    <ListItemSecondaryAction>
                      <Typography align="right">{profile.email}</Typography>
                    </ListItemSecondaryAction>
                  </ListItem>
                  <ListItem>
                    <ListItemIcon>
                      <PhoneOutlined />
                    </ListItemIcon>
                    <ListItemText
                      primary={<Typography color="secondary">Phone</Typography>}
                    />
                    <ListItemSecondaryAction>
                      <Typography align="right">{profile.contact}</Typography>
                    </ListItemSecondaryAction>
                  </ListItem>
                  <ListItem>
                    <ListItemIcon>
                      <EnvironmentOutlined />
                    </ListItemIcon>
                    <ListItemText
                      primary={
                        <Typography color="secondary">Location</Typography>
                      }
                    />
                    <ListItemSecondaryAction>
                      <Typography align="right">{profile.country}</Typography>
                    </ListItemSecondaryAction>
                  </ListItem>
                </List>
              </Grid>
            </Grid>
          </MainCard>
        )}
      </Grid>
      <Grid item xs={12} sm={7} md={8} xl={8.5}>
        {profile && (
          <Stack spacing={2.5}>
            <MainCard title="Personal Details">
              <List sx={{ py: 0 }}>
                <ListItem divider={!downMD}>
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Full Name</Typography>
                        <Typography>
                          {profile.firstName} {profile.lastName}
                        </Typography>
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Gender</Typography>
                        <Typography>{profile.gender}</Typography>
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
                <ListItem divider={!downMD}>
                  <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Country</Typography>
                        <Typography>{profile.country}</Typography>
                      </Stack>
                    </Grid>
                    <Grid item xs={12} md={6}>
                      <Stack spacing={0.5}>
                        <Typography color="secondary">Postcode</Typography>
                        <Typography>{profile.postcode}</Typography>
                      </Stack>
                    </Grid>
                  </Grid>
                </ListItem>
                <ListItem>
                  <Stack spacing={0.5}>
                    <Typography color="secondary">Address</Typography>
                    <Typography>
                      {`${profile.number ? profile.number + ", " : ""}${profile.street ? profile.street + ", " : ""}${profile.city ? profile.city : ""}`}
                    </Typography>
                  </Stack>
                </ListItem>
              </List>
            </MainCard>
            <MainCard title="About me">
              <Typography color="secondary">{profile.about}</Typography>
            </MainCard>
          </Stack>
        )}
      </Grid>
    </Grid>
  );
}
