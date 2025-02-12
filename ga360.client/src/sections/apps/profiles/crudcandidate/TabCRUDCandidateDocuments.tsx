// material-ui
import { Theme } from "@mui/material/styles";
import useMediaQuery from "@mui/material/useMediaQuery";
import Divider from "@mui/material/Divider";
import Grid from "@mui/material/Grid";
import Stack from "@mui/material/Stack";

// project import
import MainCard from "components/MainCard";

// assets
import DownloadOutlined from "@ant-design/icons/DownloadOutlined";

import defaultImages from "assets/images/users/default.png";
import { DocumentViewDataProps, TableDataProps } from "types/table";
import { useMemo, useState, useEffect, useContext } from "react";
import { useParams } from "react-router";

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
  Button,
  IconButton,
  InputLabel,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableFooter,
  TableHead,
  TableRow,
  Tooltip,
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
import { getDocumentsByUser, getUserById, updateCustomerWithDocuments } from "api/customer";
import { DocumentFileModel } from "types/customerApiModel";
import CandidateProfile from "./CandidateCRUDProfile";
import MyQualificationsProfile from "./MyQualificationsCRUDProfile";
import DuendeContext from "contexts/DuendeContext";
import MultipleFileUploader from "components/MultipleFileUploader";
//import Button from 'themes/overrides/Button';

interface ReactTableProps {
  columns: ColumnDef<DocumentViewDataProps>[];
  data: DocumentViewDataProps[];
}
export default function TabCRUDCandidateDocuments() {
  const { id } = useParams();

  const [candidate, setCandidate] = useState<CustomerListExtended>(null);
  const [documents, setDocuments] = useState<DocumentViewDataProps[]>([]);
  const [documentFiles, setDocumentFiles] = useState<File[]>([]);

  const [avatar, setAvatar] = useState<string | undefined>(
    candidate?.avatarImage ? candidate.avatarImage : defaultImages
  );

  const [isAdmin, setIsAdmin] = useState<boolean>(true);

  const columns = useMemo<ColumnDef<DocumentViewDataProps>[]>(
    () => [
      {
        header: "Document Name",
        footer: "Document Name",
        accessorKey: "name",
        enableSorting: false,
      },
      {
        accessorKey: "url",
        header: " ",
        meta: {
          className: "cell-right",
        },
        disableSortBy: true,
        cell: ({ row }) => {
          const url = row.original.url;
          return (
            <Stack
              direction="row"
              alignItems="center"
              justifyContent="flex-end" // Aligns actions to the right
              spacing={0}
            >
              <IconButton size="medium">
                <a href={url} target="_blank" rel="noopener noreferrer">
                  <DownloadOutlined />
                </a>
              </IconButton>
              <Tooltip title="Delete">
                <IconButton
                  color="error"
                  onClick={(e: React.MouseEvent<HTMLButtonElement>) => {
                    e.stopPropagation();
                  }}
                />
              </Tooltip>
            </Stack>
          );
        },
      },
    ],
    []
  );

  const { user, isLoggedIn } = useContext(DuendeContext);

  const fetchDocuments = async () => {
    try {

      const learner = await getUserById(Number(id));
      console.log(learner)
      setAvatar(learner.avatarImage);
      setCandidate(learner);
      setIsAdmin((learner.role === "Super Admin" || learner.role === "Training Centre"));

  
      const mappedFiles: DocumentViewDataProps[] = mapDocumentFilesToViewData(learner.fileDocuments);
  
      if (learner != null) {
        const files: File[] = learner.fileDocuments.map((doc) => new File([], doc.name));
        setDocumentFiles(files);
      }
      setDocuments(mappedFiles);
    } catch (error) {
      console.error("Error fetching countries:", error);
    }
  };
  
  function mapDocumentFilesToViewData(
    files: DocumentFileModel[]
  ): DocumentViewDataProps[] {
    return files.map((file) => {
      const nameParts = file.name.split("/");
      const fileName = nameParts[nameParts.length - 1];
      return {
        name: fileName,
        url: file.url,
      };
    });
  }

  useEffect(() => {
      fetchDocuments();
  }, []);

  const handleFilesUpload = async (files: File[]) => {
    
    await updateCustomerWithDocuments(candidate.id, candidate, files);

    fetchDocuments();
  };

  const handleFileRemove = async (file: File) => {
    console.log("REMOVE");
  
    // Filter out the file to be removed
    const docFiles = documentFiles.filter((doc) => doc.name !== file.name);
  
    // Update the state with the new file list
    setDocumentFiles(docFiles);
  
    // Call the update function after the state is set
    await updateCustomerWithDocuments(candidate.id, candidate, docFiles);

    fetchDocuments();
  };
  
  
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
      <MainCard title={matchDownSM ? "Sorting" : "Documents"} content={false}>
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
            {candidate &&
            <CandidateProfile
              candidate={candidate}
              defaultImages={avatar}
            ></CandidateProfile>}
          </Grid>
          {!isAdmin &&
          <Grid item xs={12}>
            <MyQualificationsProfile userId={Number(id)} />
          </Grid>
          }
        </Grid>
      </Grid>
      <Grid item xs={12} sm={7} md={8} xl={9}>
        <Grid item sx={{paddingBottom:3}}>
        <MainCard title="Document Uploader">
          <Grid container>
            <Grid item xs={12}>
              {documentFiles && 
              <MultipleFileUploader
                onFilesUpload={handleFilesUpload}
                onFileRemove={handleFileRemove}
                // @ts-ignore

                detailedFiles={documentFiles}
                initialFiles={documentFiles}
                acceptedFiles="image/*"
              />}
            </Grid>
          </Grid>
        </MainCard>
        </Grid>
        <Grid item>
        <ReactTable data={documents} columns={columns} />
        </Grid>
      </Grid>
    </Grid>
  );
}
