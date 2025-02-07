import React, { useContext } from "react";
import {
  Grid,
  Stack,
  Typography,
  List,
  ListItem,
  ListItemIcon,
  ListItemSecondaryAction,
} from "@mui/material";

import AimOutlined from "@ant-design/icons/AimOutlined";
import PhoneOutlined from "@ant-design/icons/PhoneOutlined";
import DownloadOutlined from "@ant-design/icons/DownloadOutlined";
import Avatar from "components/@extended/Avatar";
import MailOutlined from "@ant-design/icons/MailOutlined";

import MainCard from "components/MainCard";
import DuendeContext from "contexts/DuendeContext";
import { UserOutlined } from "@ant-design/icons";

const CandidateCRUDProfile = ({ candidate, defaultImages }) => {
  const { user, isLoggedIn } = useContext(DuendeContext);

  return (
    <MainCard>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <Stack spacing={2.5} alignItems="center">
            <Avatar alt="Avatar 1" size="xl" src={defaultImages} />
            <Stack spacing={0.5} alignItems="center">
              {candidate !== null ? (
                <Typography variant="h5">
                  {candidate.firstName} {candidate.lastName}
                </Typography>
              ) : (
                <></>
              )}
              <Typography color="secondary">
                {candidate != null ? (
                  candidate.employeeStatus === "employed" ? (
                    "Employed"
                  ) : candidate.employeeStatus === "selfEmployed" ? (
                    "Self Employed"
                  ) : candidate.employeeStatus === "unemployed" ? (
                    "Unemployed"
                  ) : (
                    <></>
                  )
                ) : (
                  <></>
                )}
              </Typography>
            </Stack>
          </Stack>
        </Grid>

        <Grid item xs={12}>
          <List
            component="nav"
            aria-label="main mailbox folders"
            sx={{ py: 0, "& .MuiListItem-root": { p: 0, py: 1 } }}
          >
            <ListItem>
              <ListItemIcon>
                <MailOutlined />
              </ListItemIcon>
              <ListItemSecondaryAction>
                <Typography align="right">
                  {candidate !== null ? candidate.email : <></>}
                </Typography>
              </ListItemSecondaryAction>
            </ListItem>
            <ListItem>
              <ListItemIcon>
                <PhoneOutlined />
              </ListItemIcon>
              <ListItemSecondaryAction>
                <Typography align="right">
                  {candidate !== null ? candidate.contact : <></>}
                </Typography>
              </ListItemSecondaryAction>
            </ListItem>
            <ListItem>
              <ListItemIcon>
                <AimOutlined />
              </ListItemIcon>
              <ListItemSecondaryAction>
                <Typography align="right">
                  {candidate !== null ? candidate.city : <></>}
                </Typography>
              </ListItemSecondaryAction>
            </ListItem>
            <ListItem>
              <ListItemIcon>
                <UserOutlined />
              </ListItemIcon>
              <ListItemSecondaryAction>
                <Typography align="right">
                  {candidate !== null ? candidate.role : user.role}
                </Typography>
              </ListItemSecondaryAction>
            </ListItem>
          </List>
        </Grid>
      </Grid>
    </MainCard>
  );
};

export default CandidateCRUDProfile;
