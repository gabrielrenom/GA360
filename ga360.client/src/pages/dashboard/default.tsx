// @ts-nocheck

// material-ui
import AvatarGroup from "@mui/material/AvatarGroup";
import Button from "@mui/material/Button";
import List from "@mui/material/List";
import ListItemAvatar from "@mui/material/ListItemAvatar";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemSecondaryAction from "@mui/material/ListItemSecondaryAction";
import ListItemText from "@mui/material/ListItemText";
import CircularProgress from "@mui/material/CircularProgress";
import { useNavigate } from "react-router-dom";
// project import
import MainCard from "components/MainCard";
import AnalyticEcommerce from "components/cards/statistics/AnalyticEcommerce";
import MonthlyBarChart from "sections/dashboard/default/MonthlyBarChart";
import ReportAreaChart from "sections/dashboard/default/ReportAreaChart";
import UniqueVisitorCard from "sections/dashboard/default/UniqueVisitorCard";
import SaleReportCard from "sections/dashboard/default/SaleReportCard";
import OrdersTable from "sections/dashboard/default/OrdersTable";

// assets
import GiftOutlined from "@ant-design/icons/GiftOutlined";
import MessageOutlined from "@ant-design/icons/MessageOutlined";
import SettingOutlined from "@ant-design/icons/SettingOutlined";
import avatar1 from "assets/images/users/avatar-1.png";
import avatar2 from "assets/images/users/avatar-2.png";
import avatar3 from "assets/images/users/avatar-3.png";
import avatar4 from "assets/images/users/avatar-4.png";
import { getInputValueAsString } from "@mui/base/unstable_useNumberInput/useNumberInput";
import { DashboardStats, getDashboardStats } from "api/dashboardService";
import {
  Avatar,
  Chip,
  Grid,
  IconButton,
  Paper,
  Stack,
  Tooltip,
  Typography,
} from "@mui/material";
import Box from "@mui/system/Box";
import { Suspense, useEffect, useMemo, useState, lazy } from "react";
import { styled } from "@mui/material/styles";
import { CustomerList } from "../../types/customer";
import CustomerTable from "../../sections/apps/customer/CustomerTable";
import { PatternFormat } from "react-number-format";
import EditOutlined from "@ant-design/icons/EditOutlined";
import PlusOutlined from "@ant-design/icons/PlusOutlined";
import EyeOutlined from "@ant-design/icons/EyeOutlined";
import DeleteOutlined from "@ant-design/icons/DeleteOutlined";
import { ImagePath, getImageUrl } from "../../utils/getImageUrl";
import { IndeterminateCheckbox } from "../../components/third-party/react-table";
import { ColumnDef } from "@tanstack/react-table";
import { color } from "framer-motion";
import { getDashboardStatuses } from "api/dashboardService";
import LabelledIndustries from "sections/dashboard/analytics/LabelledIndustries";
// import ButtonBase from "themes/overrides/ButtonBase";
import { ButtonBase } from "@mui/material";

const Candidates = lazy(() => import("pages/backoffice/candidates"));

// avatar style
const avatarSX = {
  width: 36,
  height: 36,
  fontSize: "1rem",
};

// action style
const actionSX = {
  mt: 0.75,
  ml: 1,
  top: "auto",
  right: "auto",
  alignSelf: "flex-start",
  transform: "none",
};

// ==============================|| DASHBOARD - DEFAULT ||============================== //

export default function DashboardDefault() {
  const [allowedCustomers, setAllowedCustomers] = useState<
    CustomerList[] | undefined
  >(undefined);

  const [open, setOpen] = useState<boolean>(false);

  const [stats, setStats] = useState<DashboardStats>(null);
  const [triggerAddCandidate, setTriggerAddCandidate] =
    useState<boolean>(false);
  const navigate = useNavigate();

  const handleNavigateBatchUpload = () => {
    navigate("/backoffice/candidatebatchuploader");
  };

  const handleNavigateQualifications = () => {
    navigate("/backoffice/qualifications");
  };

  const handleClose = () => {
    setOpen(!open);
  };

  const columns = useMemo<ColumnDef<CustomerList>[]>(
    () => [
      {
        id: "select",
        header: ({ table }) => (
          <IndeterminateCheckbox
            {...{
              checked: table.getIsAllRowsSelected(),
              indeterminate: table.getIsSomeRowsSelected(),
              onChange: table.getToggleAllRowsSelectedHandler(),
            }}
          />
        ),
        cell: ({ row }) => (
          <IndeterminateCheckbox
            {...{
              checked: row.getIsSelected(),
              disabled: !row.getCanSelect(),
              indeterminate: row.getIsSomeSelected(),
              onChange: row.getToggleSelectedHandler(),
            }}
          />
        ),
      },
      {
        header: "#",
        accessorKey: "id",
        meta: {
          className: "cell-center",
        },
      },
      {
        header: "User Info",
        accessorKey: "name",
        cell: ({ row, getValue }) => (
          <Stack direction="row" spacing={1.5} alignItems="center">
            <Avatar
              alt="Avatar 1"
              size="sm"
              src={getImageUrl(
                `avatar-${!row.original.avatar ? 1 : row.original.avatar}.png`,
                ImagePath.USERS
              )}
            />
            <Stack spacing={0}>
              <Typography variant="subtitle1">
                {getValue() as string}
              </Typography>
              <Typography color="text.secondary">
                {row.original.email as string}
              </Typography>
            </Stack>
          </Stack>
        ),
      },
      {
        header: "Contact",
        accessorKey: "contact",
        cell: ({ getValue }) => (
          <PatternFormat
            displayType="text"
            format="+1 (###) ###-####"
            mask="_"
            defaultValue={getValue() as number}
          />
        ),
      },
      {
        header: "Age",
        accessorKey: "age",
        meta: {
          className: "cell-right",
        },
      },
      {
        header: "Country",
        accessorKey: "country",
      },
      {
        header: "Status",
        accessorKey: "status",
        cell: (cell) => {
          switch (cell.getValue()) {
            case 3:
              return (
                <Chip
                  color="error"
                  label="Rejected"
                  size="small"
                  variant="light"
                />
              );
            case 1:
              return (
                <Chip
                  color="success"
                  label="Verified"
                  size="small"
                  variant="light"
                />
              );
            case 2:
            default:
              return (
                <Chip
                  color="info"
                  label="Pending"
                  size="small"
                  variant="light"
                />
              );
          }
        },
      },
      {
        header: "Actions",
        meta: {
          className: "cell-center",
        },
        disableSortBy: true,
        cell: ({ row }) => {
          const collapseIcon =
            row.getCanExpand() && row.getIsExpanded() ? (
              <PlusOutlined style={{ transform: "rotate(45deg)" }} />
            ) : (
              <EyeOutlined />
            );
          return (
            <Stack
              direction="row"
              alignItems="center"
              justifyContent="center"
              spacing={0}
            >
              <Tooltip title="View">
                <IconButton
                  color={row.getIsExpanded() ? "error" : "secondary"}
                  onClick={row.getToggleExpandedHandler()}
                >
                  {collapseIcon}
                </IconButton>
              </Tooltip>
              <Tooltip title="Edit">
                <IconButton
                  color="primary"
                  onClick={(e: React.MouseEvent<HTMLButtonElement>) => {
                    e.stopPropagation();
                    //setSelectedCustomer(row.original);
                    //    setCustomerModal(true);
                  }}
                >
                  <EditOutlined />
                </IconButton>
              </Tooltip>
              <Tooltip title="Delete">
                <IconButton
                  color="error"
                  onClick={(e: React.MouseEvent<HTMLButtonElement>) => {
                    e.stopPropagation();
                    setOpen(true);
                    //    setCustomerDeleteId(Number(row.original.id));
                  }}
                >
                  <DeleteOutlined />
                </IconButton>
              </Tooltip>
            </Stack>
          );
        },
      },
    ],
    []
  );

  const handleAddCandidate = () => {
    setTriggerAddCandidate(true);
  };

  const resetTriggerAddCandidate = () => {
    setTriggerAddCandidate(false);
  };

  useEffect(() => {
    const fetchCustomerData = async () => {
      try {
        const customers = await fetchCustomerList();
        setAllowedCustomers(customers);
        console.log(customers);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchCustomerData();
  }, []);

  useEffect(() => {
    const fetchDashboardStats = async () => {
      try {
        const data = await getDashboardStats();
        setStats(data);
        console.log(data);
      } catch (error) {
        console.error("Failed to fetch for the dashboard", error);
      }
    };

    fetchDashboardStats();
  }, []);

  const getColor = (percentage) => {
    if (percentage > 10) return "success";
    if (percentage > 0 && percentage <= 10) return "warning";
    return "error";
  };

  return (
    <Grid container rowSpacing={4.5} columnSpacing={2.75}>
      {/* row 0 */}
      <Box sx={{ flexGrow: 1, paddingLeft: 3, paddingTop: 2 }}>
        <Grid container spacing={2}>
          {/* Main Card on the left */}
          {/* <Grid item xs={12} sm={6} md={5} lg={4}>
            <Grid container>
              <Grid item>
                <MainCard
                  sx={{
                    height: "100%",
                    backgroundColor: "navy",
                    color: "white",
                  }}
                >
                  <Stack spacing={2}>
                    <Typography variant="h1">Latest news from CQG</Typography>
                    <Typography variant="body1" noWrap>
                      New qualification Alert: Level 4 Shop Fitting
                    </Typography>
                    <Button variant="outlined" color="info">
                      Apply Now
                    </Button>
                  </Stack>
                </MainCard>
              </Grid>
            </Grid>
          </Grid> */}

          {/* 2x2 grid of cards on the right */}
          <Grid item xs={12}>
            <Grid container spacing={2}>
              <Grid item xs={12} sm={3}>
                <MainCard
                  sx={{
                    backgroundColor: "#2E5A88",
                    color: "white",
                  }}
                >
                  <Stack spacing={2}>
                    <ButtonBase onClick={handleAddCandidate}>
                      <Typography variant="body1" noWrap>
                        {" "}
                        Add New Learner{" "}
                      </Typography>
                    </ButtonBase>
                  </Stack>
                </MainCard>
              </Grid>

              <Grid item xs={12} sm={3}>
                <MainCard
                  sx={{
                    backgroundColor: "#2E5A88",
                    color: "white",
                  }}
                >
                  <Stack spacing={2}>
                    <ButtonBase onClick={handleNavigateBatchUpload}>
                      <Typography variant="body1" noWrap>
                        {" "}
                        Batch Upload (csv){" "}
                      </Typography>
                    </ButtonBase>
                  </Stack>
                </MainCard>
              </Grid>

              <Grid item xs={12} sm={3}>
                <MainCard
                  sx={{
                    backgroundColor: "#2E5A88",
                    color: "white",
                  }}
                >
                  <Stack spacing={2}>
                    <ButtonBase onClick={handleNavigateQualifications}>
                      <Typography variant="body1" noWrap>
                        {" "}
                        Qualifications{" "}
                      </Typography>
                    </ButtonBase>
                  </Stack>
                </MainCard>
              </Grid>
              <Grid item xs={12} sm={3}>
                <MainCard
                  sx={{
                    backgroundColor: "#2E5A88",
                    color: "white",
                    opacity: 0.5, // Reduce the opacity to give a disabled look
                    pointerEvents: "none", // Disable any pointer events
                  }}
                >
                  <Stack spacing={2}>
                    <ButtonBase>
                      <Typography variant="body1" noWrap>
                        Reports
                      </Typography>
                    </ButtonBase>
                  </Stack>
                </MainCard>
              </Grid>
            </Grid>
          </Grid>
        </Grid>
      </Box>
      <Grid item md={8} sx={{ display: { sm: "none", md: "block" } }} />

      {/* row 1 */}

      <Grid container spacing={3} sx={{ flexGrow: 1, paddingLeft: 3 }}>
        {stats &&
          stats.find(
            (stat) =>
              stat.statisticTypeDescription === "CANDIDATE_REGISTRATIONS"
          ) && (
            <Grid item xs={12} sm={6} md={4} lg={3}>
              <AnalyticEcommerce
                title="Candidate Registrations"
                count={
                  stats.find(
                    (stat) =>
                      stat.statisticTypeDescription ===
                      "CANDIDATE_REGISTRATIONS"
                  )?.total
                }
                percentage={
                  stats.find(
                    (stat) =>
                      stat.statisticTypeDescription ===
                      "CANDIDATE_REGISTRATIONS"
                  )?.percentage
                }
                extra={
                  stats.find(
                    (stat) =>
                      stat.statisticTypeDescription ===
                      "CANDIDATE_REGISTRATIONS"
                  )?.totalYear
                }
                color={getColor(
                  stats.find(
                    (stat) =>
                      stat.statisticTypeDescription ===
                      "CANDIDATE_REGISTRATIONS"
                  )?.percentage
                )}
              />
            </Grid>
          )}
        {stats &&
          stats.find(
            (stat) => stat.statisticTypeDescription === "NEW_LEARNERS"
          ) && (
            <Grid item xs={12} sm={6} md={4} lg={3}>
              <AnalyticEcommerce
                title="New Learners"
                count={
                  stats.find(
                    (stat) => stat.statisticTypeDescription === "NEW_LEARNERS"
                  )?.total
                }
                percentage={
                  stats.find(
                    (stat) => stat.statisticTypeDescription === "NEW_LEARNERS"
                  )?.percentage
                }
                extra={
                  stats.find(
                    (stat) => stat.statisticTypeDescription === "NEW_LEARNERS"
                  )?.totalYear
                }
                color={getColor(
                  stats.find(
                    (stat) => stat.statisticTypeDescription === "NEW_LEARNERS"
                  )?.percentage
                )}
              />
            </Grid>
          )}
        {stats &&
          stats.find(
            (stat) => stat.statisticTypeDescription === "ACTIVE_LEARNERS"
          ) && (
            <Grid item xs={12} sm={6} md={4} lg={3}>
              <AnalyticEcommerce
                title="Active Learners"
                count={
                  stats.find(
                    (stat) =>
                      stat.statisticTypeDescription === "ACTIVE_LEARNERS"
                  )?.total
                }
                percentage={
                  stats.find(
                    (stat) =>
                      stat.statisticTypeDescription === "ACTIVE_LEARNERS"
                  )?.percentage
                }
                extra={
                  stats.find(
                    (stat) =>
                      stat.statisticTypeDescription === "ACTIVE_LEARNERS"
                  )?.totalYear
                }
                color={getColor(
                  stats.find(
                    (stat) =>
                      stat.statisticTypeDescription === "ACTIVE_LEARNERS"
                  )?.percentage
                )}
              />
            </Grid>
          )}
        {stats &&
          stats.find(
            (stat) => stat.statisticTypeDescription === "COMPLETED_LEARNERS"
          ) && (
            <Grid item xs={12} sm={6} md={4} lg={3}>
              <AnalyticEcommerce
                title="Completed Learners"
                count={
                  stats.find(
                    (stat) =>
                      stat.statisticTypeDescription === "COMPLETED_LEARNERS"
                  )?.total
                }
                percentage={
                  stats.find(
                    (stat) =>
                      stat.statisticTypeDescription === "COMPLETED_LEARNERS"
                  )?.percentage
                }
                extra={
                  stats.find(
                    (stat) =>
                      stat.statisticTypeDescription === "COMPLETED_LEARNERS"
                  )?.totalYear
                }
                color={getColor(
                  stats.find(
                    (stat) =>
                      stat.statisticTypeDescription === "COMPLETED_LEARNERS"
                  )?.percentage
                )}
              />
            </Grid>
          )}
      </Grid>
      <Grid
        item
        md={8}
        sx={{ display: { sm: "none", md: "block", lg: "none" } }}
      />
      <LabelledIndustries />
      {/* row 2 */}
      {/* <Grid item xs={12} md={7} lg={8}>
        <UniqueVisitorCard />
      </Grid>
      <Grid item xs={12} md={5} lg={4}>
        <Grid container alignItems="center" justifyContent="space-between">
          <Grid item>
            <Typography variant="h5">Income Overview</Typography>
          </Grid>
          <Grid item />
        </Grid>
        <MainCard sx={{ mt: 2 }} content={false}>
          <Box sx={{ p: 3, pb: 0 }}>
            <Stack spacing={2}>
              <Typography variant="h6" color="text.secondary">
                This Week Statistics
              </Typography>
              <Typography variant="h3">$7,650</Typography>
            </Stack>
          </Box>
          <MonthlyBarChart />
        </MainCard>
      </Grid> */}

      {/* row 3 */}
      <Grid item xs={12} md={12} lg={12}>
        <Grid container alignItems="center" justifyContent="space-between">
          <Grid item>
            <Typography variant="h5">Candidates</Typography>
          </Grid>
          <Grid item />
        </Grid>
        <Suspense fallback={<CircularProgress />}>
          <Candidates
            triggerAddCandidate={triggerAddCandidate}
            onModalClose={resetTriggerAddCandidate}
          />
        </Suspense>
        {/* <MainCard sx={{ mt: 2 }} content={false}>
          {allowedCustomers === undefined ? (
            <>Loading...</>
          ) : (
            <CustomerTable
              data={allowedCustomers}
              columns={columns}
              modalToggler={() => {
                //setCustomerModal(true);
                //setSelectedCustomer(null);
              }}
            />
          )}
        </MainCard> */}
        {/*<MainCard sx={{ mt: 2 }} content={false}>*/}
        {/*    <OrdersTable />*/}
        {/*</MainCard>*/}
      </Grid>

      {/* row 4 */}
      {/* <Grid item xs={12} md={7} lg={8}>
        <SaleReportCard />
      </Grid>
      <Grid item xs={12} md={5} lg={4}>
        <Grid container alignItems="center" justifyContent="space-between">
          <Grid item>
            <Typography variant="h5">Transaction History</Typography>
          </Grid>
          <Grid item />
        </Grid>
        <MainCard sx={{ mt: 2 }} content={false}>
          <List
            component="nav"
            sx={{
              px: 0,
              py: 0,
              "& .MuiListItemButton-root": {
                py: 1.5,
                "& .MuiAvatar-root": avatarSX,
                "& .MuiListItemSecondaryAction-root": {
                  ...actionSX,
                  position: "relative",
                },
              },
            }}
          >
            <ListItemButton divider>
              <ListItemAvatar>
                <Avatar
                  sx={{ color: "success.main", bgcolor: "success.lighter" }}
                >
                  <GiftOutlined />
                </Avatar>
              </ListItemAvatar>
              <ListItemText
                primary={
                  <Typography variant="subtitle1">Order #002434</Typography>
                }
                secondary="Today, 2:00 AM"
              />
              <ListItemSecondaryAction>
                <Stack alignItems="flex-end">
                  <Typography variant="subtitle1" noWrap>
                    + $1,430
                  </Typography>
                  <Typography variant="h6" color="secondary" noWrap>
                    78%
                  </Typography>
                </Stack>
              </ListItemSecondaryAction>
            </ListItemButton>
            <ListItemButton divider>
              <ListItemAvatar>
                <Avatar
                  sx={{ color: "primary.main", bgcolor: "primary.lighter" }}
                >
                  <MessageOutlined />
                </Avatar>
              </ListItemAvatar>
              <ListItemText
                primary={
                  <Typography variant="subtitle1">Order #984947</Typography>
                }
                secondary="5 August, 1:45 PM"
              />
              <ListItemSecondaryAction>
                <Stack alignItems="flex-end">
                  <Typography variant="subtitle1" noWrap>
                    + $302
                  </Typography>
                  <Typography variant="h6" color="secondary" noWrap>
                    8%
                  </Typography>
                </Stack>
              </ListItemSecondaryAction>
            </ListItemButton>
            <ListItemButton>
              <ListItemAvatar>
                <Avatar sx={{ color: "error.main", bgcolor: "error.lighter" }}>
                  <SettingOutlined />
                </Avatar>
              </ListItemAvatar>
              <ListItemText
                primary={
                  <Typography variant="subtitle1">Order #988784</Typography>
                }
                secondary="7 hours ago"
              />
              <ListItemSecondaryAction>
                <Stack alignItems="flex-end">
                  <Typography variant="subtitle1" noWrap>
                    + $682
                  </Typography>
                  <Typography variant="h6" color="secondary" noWrap>
                    16%
                  </Typography>
                </Stack>
              </ListItemSecondaryAction>
            </ListItemButton>
          </List>
        </MainCard>
        <MainCard sx={{ mt: 2 }}>
          <Stack spacing={3}>
            <Grid container justifyContent="space-between" alignItems="center">
              <Grid item>
                <Stack>
                  <Typography variant="h5" noWrap>
                    Help & Support Chat
                  </Typography>
                  <Typography variant="caption" color="secondary" noWrap>
                    Typical replay within 5 min
                  </Typography>
                </Stack>
              </Grid>
              <Grid item>
                <AvatarGroup
                  sx={{ "& .MuiAvatar-root": { width: 32, height: 32 } }}
                >
                  <Avatar alt="Remy Sharp" src={avatar1} />
                  <Avatar alt="Travis Howard" src={avatar2} />
                  <Avatar alt="Cindy Baker" src={avatar3} />
                  <Avatar alt="Agnes Walker" src={avatar4} />
                </AvatarGroup>
              </Grid>
            </Grid>
            <Button
              size="small"
              variant="contained"
              sx={{ textTransform: "capitalize" }}
            >
              Need Help?
            </Button>
          </Stack>
        </MainCard>
      </Grid> */}
    </Grid>

    //<div>

    //    {allowedCustomers === undefined ? <>Loading...</> :
    //        <CustomerTable data={allowedCustomers} columns={columns} modalToggler={() => {
    //            setCustomerModal(true);
    //            setSelectedCustomer(null);
    //        }} />
    //    }
    //</div>
  );

  async function fetchCustomerList() {
    const response = await fetch("/api/customer/list", {
      headers: {
        "X-CSRF": "Dog",
      },
    });
    const result = await response.json();
    return result;
  }
}
