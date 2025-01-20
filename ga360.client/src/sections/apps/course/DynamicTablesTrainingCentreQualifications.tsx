// material-ui
import { Theme } from "@mui/material/styles";
import useMediaQuery from "@mui/material/useMediaQuery";
import Divider from "@mui/material/Divider";

import Stack from "@mui/material/Stack";

// project import
import MainCard from "components/MainCard";

import {
  QualificationLearnerViewDataProps,
  QualificationTrainingCentreViewDataPropsExtended,
  QualificationViewDataProps,
  TableDataProps,
} from "types/table";
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
import { QualificationModel } from "types/customerApiModel";
import { useParams } from "react-router";
import {
  getQualificationsWithTrainingCentresForTable,
  QualificationExtended,
  QualificationTable,
} from "api/qualificationService";
import { PlusOutlined } from "@ant-design/icons";
import DynamicTableCustomerWithCourseQualificationRecords from "sections/apps/course/DynamicTableCustomerWithCourseQualificationRecords";
import DuendeContext from "contexts/DuendeContext";

interface ReactTableProps {
  columns: ColumnDef<QualificationTrainingCentreViewDataPropsExtended>[];
  data: QualificationTrainingCentreViewDataPropsExtended[];
  onClose: () => void; // Callback function to be called when the dialog is closed
}

export default function DynamicTablesTrainingCentreQualifications() {
  const { user } = useContext(DuendeContext);

  const [qualifications, setQualifications] = useState<
    QualificationTrainingCentreViewDataPropsExtended[]
  >([]);

  const mapQualifications = (
    qualifications: QualificationTable[]
  ): QualificationTrainingCentreViewDataPropsExtended[] => {
    // @ts-ignore
    return qualifications.map((certificate) => ({
      name: certificate.name,
      regDate: certificate.registrationDate
        ? new Date(certificate.registrationDate).toLocaleDateString("en-GB")
        : "", // Display an empty string if the date is null
      status: certificate.status.toString(), // Assuming status is converted to string
      qan: certificate.qan,
      price: certificate.price,
      completeDate: certificate.certificateDate
        ? new Date(certificate.certificateDate).toLocaleDateString("en-GB")
        : "",
      internalReference: certificate.internalReference,
      awardingBody: certificate.awardingBody,
      learners: certificate.learners,
      assessors: 0,
      expectedDate: certificate.expectedDate
      ? new Date(certificate.expectedDate).toLocaleDateString("en-GB")
      : "",
      certificateNumber: certificate.certificateNumber,
      id: certificate.id,
      registrationDate:  certificate.registrationDate,
      trainingCentreId: certificate.trainingCentreId,
      trainingCentre: certificate.trainingCentre,
      sector: certificate.sector,
      qualificationName: certificate.name,
      expirationDate: certificate.expectedDate,
      certificateDate: certificate.certificateDate
    }));
  };

  const columns = useMemo<
    ColumnDef<QualificationTrainingCentreViewDataPropsExtended>[]
  >(
    () => [
      {
        header: "QAN",
        footer: "QAN",
        accessorKey: "qan",
      },
      {
        header: "Internal Reference",
        footer: "Internal Reference",
        accessorKey: "internalReference",
      },
      {
        header: "Qualification Name",
        footer: "Qualification Name",
        accessorKey: "name",
        enableSorting: true,
      },
      {
        header: "Awarding Body",
        footer: "Awarding Body",
        accessorKey: "awardingBody",
      },
      {
        header: "Learners",
        footer: "Learners",
        accessorKey: "learners",
      },
      {
        header: "Assessors",
        footer: "Assessors",
        accessorKey: "assessors",
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
        header: "Cert Number",
        footer: "Cert Number",
        accessorKey: "certificateNumber",
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
        header: "Price",
        footer: "Price",
        accessorKey: "price",
      },
    ],
    []
  );

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const qualifications: QualificationTable[] =
          await getQualificationsWithTrainingCentresForTable(
            Number(user.trainingCentreId)
          );

          console.log(qualifications)
        const qualificationsResponse = mapQualifications(qualifications);

        setQualifications(qualificationsResponse);
      } catch (error) {
        console.error("Error fetching countries:", error);
      }
    };

    fetchUser();
  }, []);

  const refreshTable = async () => {
    const response = await getQualificationsWithTrainingCentresForTable(
      Number(user.trainingCentreId)
    );

    console.log(response)

    const qualificationsResponse = mapQualifications(response);

    setQualifications(qualificationsResponse);
  };

  function ReactTable({ columns, data, onClose }: ReactTableProps) {
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
    };

    const handleAddQualificationClick = () => {
      window.open("https://form.jotform.com/243523175109049", "_blank");
    };

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
              color="primary"
              startIcon={<PlusOutlined />}
              onClick={handleAddQualificationClick}
            >
              Request Qualification
            </Button>

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
    <>
      <ReactTable
        data={qualifications}
        columns={columns}
        onClose={refreshTable}
      />
    </>
  );
}
