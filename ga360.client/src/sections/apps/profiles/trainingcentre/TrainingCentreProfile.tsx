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
import UserOutlined from "@ant-design/icons/UserOutlined";
import MainCard from "components/MainCard";
import DuendeContext from "contexts/DuendeContext";

const TrainingCentreProfile = () => {
  const { user, isLoggedIn } = useContext(DuendeContext);

  return (
    <MainCard>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <Stack spacing={2.5} alignItems="center">
            <Avatar alt="Avatar 1" size="xl" src={user.avatarImage} />
            <Stack spacing={0.5} alignItems="center">
              {user !== null ? (
                <Typography variant="h5">
                  {user.firstName} {user.lastName}
                </Typography>
              ) : (
                <></>
              )}
              <Typography color="secondary">
                {user != null ? user.employeeStatus : <></>}
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
                  {user !== null ? user.email : <></>}
                </Typography>
              </ListItemSecondaryAction>
            </ListItem>
            <ListItem>
              <ListItemIcon>
                <PhoneOutlined />
              </ListItemIcon>
              <ListItemSecondaryAction>
                <Typography align="right">
                  {user !== null ? user.contact : <></>}
                </Typography>
              </ListItemSecondaryAction>
            </ListItem>
            <ListItem>
              <ListItemIcon>
                <AimOutlined />
              </ListItemIcon>
              <ListItemSecondaryAction>
                <Typography align="right">
                  {user !== null ? user.city : <></>}
                </Typography>
              </ListItemSecondaryAction>
            </ListItem>
            <ListItem>
              <ListItemIcon>
                <UserOutlined />
              </ListItemIcon>
              <ListItemSecondaryAction>
                <Typography align="right">
                  {user !== null ? user.role : <></>}
                </Typography>
              </ListItemSecondaryAction>
            </ListItem>
          </List>
        </Grid>
      </Grid>
    </MainCard>
  );
};

export default TrainingCentreProfile;
