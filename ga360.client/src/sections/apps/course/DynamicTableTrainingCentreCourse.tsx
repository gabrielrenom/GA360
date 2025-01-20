// material-ui
import { Theme } from "@mui/material/styles";
import useMediaQuery from "@mui/material/useMediaQuery";
import Divider from "@mui/material/Divider";

import Stack from "@mui/material/Stack";

// project import
import MainCard from "components/MainCard";
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
import DuendeContext from "contexts/DuendeContext";
import { CourseDetails, getCoursesDetails } from "api/courseService";

interface ReactTableProps {
  columns: ColumnDef<CourseViewDataProps>[];
  data: CourseViewDataProps[];
}

interface CourseViewDataProps {
  name: string;
  description: string;
  duration: number;
  learners: number;
  expectedDate: string;
  status: number;
  sector: string;
  price: number;
}

export default function DynamicTableTrainingCentreCourse() {
  const { user } = useContext(DuendeContext);

  const [courses, setCourses] = useState<CourseViewDataProps[]>([]);

  const mapCourses = (
    coursesModel: CourseDetails[]
  ): CourseViewDataProps[] => {
    return coursesModel.map((courseItem) => ({
      name: courseItem.name,
      description: courseItem.description,
      duration: courseItem.duration,
      learners:courseItem.learners,
      expectedDate: courseItem.expectedDate
      ? new Date(courseItem.expectedDate).toLocaleDateString("en-GB")
      : "",
      status: courseItem.status,
      sector: courseItem.sector,
      price: courseItem.price
    }));
  };

  const columns = useMemo<
    ColumnDef<CourseViewDataProps>[]
  >(
    () => [
      {
        header: "Name",
        footer: "Name",
        accessorKey: "name",
      },
      {
        header: "Description",
        footer: "Description",
        accessorKey: "description",
      },
      {
        header: "Hours",
        footer: "Hours",
        accessorKey: "duration",
        enableSorting: true,
      },
      {
        header: "Learners",
        footer: "Learners",
        accessorKey: "learners",
      },
      {
        header: "Exp Date",
        footer: "Exp Date",
        accessorKey: "expectedDate",
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
        header: "Sector",
        footer: "Sector",
        accessorKey: "sector",
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
        const coursesResult: CourseDetails[] =

          await getCoursesDetails(Number(user.trainingCentreId));

        const coursesResponse = mapCourses(coursesResult);

        setCourses(coursesResponse);
      } catch (error) {
        console.error("Error fetching courses:", error);
      }
    };

    fetchUser();
  }, []);

  function ReactTable({ columns, data }: ReactTableProps) {
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
        data={courses}
        columns={columns}
      />
    </>
  );
}
