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

import defaultImages from "assets/images/users/default.png";
import {
  QualificationLearnerViewDataProps,
  QualificationViewDataProps,
  TableDataProps,
} from "types/table";
import { useEffect, useMemo, useState } from "react";
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
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Modal,
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
import { CustomerListExtended } from "types/customer";
import { getCandidate, getUserById } from "api/customer";
import CandidateProfile from "./CandidateCRUDProfile";
import { QualificationModel } from "types/customerApiModel";
import MyQualificationsProfile from "./MyQualificationsCRUDProfile";
import { useParams } from "react-router";
import {
  getQualificationDetailByUserId,
  QualificationExtended,
} from "api/qualificationService";
import { PlusOutlined } from "@ant-design/icons";
import DynamicTableCustomerWithCourseQualificationRecords from "sections/apps/course/DynamicTableCustomerWithCourseQualificationRecords";

interface ReactTableProps {
  columns: ColumnDef<QualificationLearnerViewDataProps>[];
  data: QualificationLearnerViewDataProps[];
  userId: number;
  onClose: () => void // Callback function to be called when the dialog is closed
}

export default function TabCRUDCandidateQualification() {
  const { id } = useParams();

  const [candidate, setCandidate] = useState<CustomerListExtended>(null);
  const matchDownMD = useMediaQuery((theme: Theme) =>
    theme.breakpoints.down("md")
  );
  const [qualifications, setQualifications] = useState<
    QualificationLearnerViewDataProps[]
  >([]);
  const [avatar, setAvatar] = useState<string | undefined>(
    candidate?.avatarImage ? candidate.avatarImage : defaultImages
  );
  const [isAdmin, setIsAdmin] = useState<boolean>(true);

  const mapQualifications = (
    qualifications: QualificationExtended[]
  ): QualificationLearnerViewDataProps[] => {
    return qualifications.map((certificate) => ({
      name: certificate.name,
      regDate: certificate.registrationDate
        ? new Date(certificate.registrationDate).toLocaleDateString("en-GB")
        : "", // Display an empty string if the date is null
      status: certificate.status.toString(), // Assuming status is converted to string
      assessor: certificate.assessor,
      qan: certificate.qan,
      price: certificate.price,
      completeDate: certificate.certificateDate
        ? new Date(certificate.certificateDate).toLocaleDateString("en-GB")
        : "", // Display an empty string if the date is null
      expectedDate: certificate.expectedDate
      ? new Date(certificate.expectedDate).toLocaleDateString("en-GB")
      : ""
    }));
  };
  

  const statusCellRenderer = (cell) => {
    console.log("CELL", cell);
    const status = cell.value;
    return status === "1" ? "Active" : status === "2" ? "No Active" : "Unknown";
  };

  const columns = useMemo<ColumnDef<QualificationLearnerViewDataProps>[]>(
    () => [
      {
        header: "Qualification Name",
        footer: "Qualification Name",
        accessorKey: "name",
        enableSorting: true,
      },
      {
        header: "QAN",
        footer: "QAN",
        accessorKey: "qan",
      },
      {
        header: "assessor",
        footer: "assessor",
        accessorKey: "assessor",
      },
      {
        header: "Reg Date",
        footer: "Reg Date",
        accessorKey: "regDate",
        enableSorting: true,
        meta: {
          className: "cell-right",
        },
      },
      {
        header: "Exp Date",
        footer: "Exp Date",
        accessorKey: "expectedDate",
        enableSorting: true,
        meta: {
          className: "cell-right",
        },
      },
      {
        header: "Status",
        footer: "Status",
        accessorKey: "status",
        cell: ({ getValue }) => {
          return getValue() === "1" ? "Active" : "Unknown";
        },
      },
      {
        header: "price",
        footer: "price",
        accessorKey: "price",
      },
    ],
    []
  );

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const learner = await getUserById(Number(id));
        setAvatar(learner.avatarImage);
        setIsAdmin((learner.role === "Super Admin" || learner.role === "Training Centre"));
        setCandidate(learner);

        const response = await getQualificationDetailByUserId(Number(id));
        console.log(response)
        const qualificationsResponse = mapQualifications(response);
        setQualifications(qualificationsResponse);
      } catch (error) {
        console.error("Error fetching countries:", error);
      }
    };

    fetchUser();
  }, []);

  const refreshTable = async () => {
    const response = await getQualificationDetailByUserId(Number(id));

    const qualificationsResponse = mapQualifications(response);
    setQualifications(qualificationsResponse);
  };

  function ReactTable({ columns, data, userId, onClose }: ReactTableProps) {
    const [open, setOpen] = useState(false);
    const matchDownSM = useMediaQuery((theme: Theme) =>
      theme.breakpoints.down("sm")
    );
    const [sorting, setSorting] = useState<SortingState>([
      {
        id: "name",
        desc: false,
      },
    ]);

    const table = useReactTable({
      data,
      columns,
      state: {
        sorting,
      },
      onSortingChange: setSorting,
      getCoreRowModel: getCoreRowModel(),
      getSortedRowModel: getSortedRowModel(),
      getPaginationRowModel: getPaginationRowModel(),
    });

    let headers: LabelKeyObject[] = [];
    
    table.getAllColumns().map((columns) =>
      headers.push({
        label:
          typeof columns.columnDef.header === "string"
            ? columns.columnDef.header
            : "#",
        // @ts-ignore
        key: columns.columnDef.accessorKey,
      })
    );

    const handleOpen = () => setOpen(true);
    const handleClose = () => {
        setOpen(false);
        onClose(); 
    }

    return (
      <MainCard
        title={matchDownSM ? "Sorting" : "Qualifications"}
        content={false}
        secondary={
          <Stack direction="row" alignItems="center" spacing={{ xs: 1, sm: 2 }}>
            <SelectColumnSorting
              {...{
                getState: table.getState,
                getAllColumns: table.getAllColumns,
                setSorting,
              }}
            />
            <Button
              variant="contained"
              startIcon={<PlusOutlined />}
              onClick={handleOpen}
            >
              Add Qualification
            </Button>
            <Dialog open={open} onClose={handleClose} maxWidth="md" fullWidth>
              <DialogTitle>Qualifications</DialogTitle>
              <DialogContent>
                <DynamicTableCustomerWithCourseQualificationRecords
                  customerId={userId}
                />
              </DialogContent>
              <DialogActions>
                <Button onClick={handleClose}>Close</Button>
              </DialogActions>
            </Dialog>
            <CSVExport
              {...{
                data,
                headers,
                filename: top ? "qualifications.csv" : "qualifications.csv",
              }}
            />
          </Stack>
        }
      >
        <ScrollX>
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                {table
                  .getHeaderGroups()
                  .map((headerGroup: HeaderGroup<any>) => (
                    <TableRow key={headerGroup.id}>
                      {headerGroup.headers.map((header) => {
                        if (
                          header.column.columnDef.meta !== undefined &&
                          header.column.getCanSort()
                        ) {
                          Object.assign(header.column.columnDef.meta, {
                            className:
                              header.column.columnDef.meta.className +
                              " cursor-pointer prevent-select",
                          });
                        }

                        return (
                          <TableCell
                            key={header.id}
                            {...header.column.columnDef.meta}
                            onClick={header.column.getToggleSortingHandler()}
                            {...(header.column.getCanSort() &&
                              header.column.columnDef.meta === undefined && {
                                className: "cursor-pointer prevent-select",
                              })}
                          >
                            {header.isPlaceholder ? null : (
                              <Stack
                                direction="row"
                                spacing={1}
                                alignItems="center"
                              >
                                <Box>
                                  {flexRender(
                                    header.column.columnDef.header,
                                    header.getContext()
                                  )}
                                </Box>
                                {header.column.getCanSort() && (
                                  <HeaderSort column={header.column} />
                                )}
                              </Stack>
                            )}
                          </TableCell>
                        );
                      })}
                    </TableRow>
                  ))}
              </TableHead>
              <TableBody>
                {table.getRowModel().rows.map((row) => (
                  <TableRow key={row.id}>
                    {row.getVisibleCells().map((cell) => (
                      <TableCell key={cell.id} {...cell.column.columnDef.meta}>
                        {flexRender(
                          cell.column.columnDef.cell,
                          cell.getContext()
                        )}
                      </TableCell>
                    ))}
                  </TableRow>
                ))}
              </TableBody>
              <TableFooter>
                {table.getFooterGroups().map((footerGroup) => (
                  <TableRow key={footerGroup.id}>
                    {footerGroup.headers.map((footer) => (
                      <TableCell
                        key={footer.id}
                        {...footer.column.columnDef.meta}
                      >
                        {footer.isPlaceholder
                          ? null
                          : flexRender(
                              footer.column.columnDef.header,
                              footer.getContext()
                            )}
                      </TableCell>
                    ))}
                  </TableRow>
                ))}
              </TableFooter>
            </Table>
          </TableContainer>

          <Divider />
          <Box sx={{ p: 2 }}>
            <TablePagination
              {...{
                setPageSize: table.setPageSize,
                setPageIndex: table.setPageIndex,
                getState: table.getState,
                getPageCount: table.getPageCount,
              }}
            />
          </Box>
        </ScrollX>
      </MainCard>
    );
  }

  return (
    <Grid container spacing={3}>
      <Grid item xs={12} sm={5} md={4} xl={3}>
        <Grid container spacing={3}>
          <Grid item xs={12}>
            <CandidateProfile
              candidate={candidate}
              defaultImages={avatar}
            ></CandidateProfile>
          </Grid>
          {!isAdmin &&
          <Grid item xs={12}>
            <MyQualificationsProfile userId={Number(id)} />
          </Grid>
          }
        </Grid>
      </Grid>
      <Grid item xs={12} sm={7} md={8} xl={9}>
        <ReactTable data={qualifications} columns={columns} userId={Number(id)} onClose={refreshTable} />
      </Grid>
    </Grid>
  );
}
