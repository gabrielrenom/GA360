import React, { useState, useEffect } from 'react';
import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Pagination, Box, CircularProgress, TableSortLabel } from '@mui/material';
import { AuditOutlined } from '@ant-design/icons';

const logsPerPage = 10;

const AuditTrail = () => {
  const [logs, setLogs] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [order, setOrder] = useState('asc');
  const [orderBy, setOrderBy] = useState('createdAt');

  useEffect(() => {
    fetchLogs();
  }, []);

  const fetchLogs = async () => {
    try {
      setLoading(true);
      console.log("audit");
      const response = await fetch('/api/audittrail', {
        headers: {
          "X-CSRF": "Dog",
        },
      });
      const data = await response.json();
      setLogs(data);
    } catch (error) {
      console.error('Error fetching logs:', error);
    } finally {
      setLoading(false);
    }
  };

  const handlePageChange = (event, value) => {
    setCurrentPage(value);
  };

  const handleSort = (property) => {
    const isAsc = orderBy === property && order === 'asc';
    setOrder(isAsc ? 'desc' : 'asc');
    setOrderBy(property);
  };

  const sortedLogs = logs.sort((a, b) => {
    if (orderBy === 'createdAt') {
        // @ts-ignore
      return order === 'asc' ? new Date(a.createdAt) - new Date(b.createdAt) : new Date(b.createdAt) - new Date(a.createdAt);
    }
    return order === 'asc' ? a[orderBy].localeCompare(b[orderBy]) : b[orderBy].localeCompare(a[orderBy]);
  });

  const totalPages = Math.ceil(sortedLogs.length / logsPerPage);
  const currentLogs = sortedLogs.slice((currentPage - 1) * logsPerPage, currentPage * logsPerPage);

  const getFontColor = (level) => {
    switch (level) {
      case 'Warning':
        return 'orange';
      case 'Error':
        return 'red';
      default:
        return 'inherit';
    }
  };

  return (
    <Paper elevation={3} style={{ padding: 16 }}>
      {loading ? (
        <Box display="flex" justifyContent="center" alignItems="center" height="100vh">
          <CircularProgress />
          <Box ml={2}>Loading...</Box>
        </Box>
      ) : (
        <>
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>
                    <TableSortLabel
                      active={orderBy === 'id'}
                              // @ts-ignore

                      direction={orderBy === 'id' ? order : 'asc'}
                      onClick={() => handleSort('id')}
                    >
                      ID
                    </TableSortLabel>
                  </TableCell>
                  <TableCell>
                    <TableSortLabel
                      active={orderBy === 'description'}
                              // @ts-ignore

                      direction={orderBy === 'description' ? order : 'asc'}
                      onClick={() => handleSort('description')}
                    >
                      Message
                    </TableSortLabel>
                  </TableCell>
                  <TableCell>
                    <TableSortLabel
                      active={orderBy === 'area'}
                              // @ts-ignore

                      direction={orderBy === 'area' ? order : 'asc'}
                      onClick={() => handleSort('area')}
                    >
                      Area
                    </TableSortLabel>
                  </TableCell>
                  <TableCell>
                    <TableSortLabel
                      active={orderBy === 'level'}
                              // @ts-ignore

                      direction={orderBy === 'level' ? order : 'asc'}
                      onClick={() => handleSort('level')}
                    >
                      Type
                    </TableSortLabel>
                  </TableCell>
                  <TableCell>
                    <TableSortLabel
                      active={orderBy === 'from'}
                              // @ts-ignore

                      direction={orderBy === 'from' ? order : 'asc'}
                      onClick={() => handleSort('from')}
                    >
                      By
                    </TableSortLabel>
                  </TableCell>
                  <TableCell>
                    <TableSortLabel
                      active={orderBy === 'createdAt'}
                              // @ts-ignore

                      direction={orderBy === 'createdAt' ? order : 'asc'}
                      onClick={() => handleSort('createdAt')}
                    >
                      Created At
                    </TableSortLabel>
                  </TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {currentLogs.map((log) => (
                  <TableRow key={log.id}>
                    <TableCell>{log.id}</TableCell>
                    <TableCell style={{ color: getFontColor(log.level) }}>{log.description}</TableCell>
                    <TableCell>{log.area}</TableCell>
                    <TableCell>{log.level}</TableCell>
                    <TableCell>{log.from}</TableCell>
                    <TableCell>{new Date(log.createdAt).toLocaleString('en-GB')}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
          <Box display="flex" justifyContent="center" mt={2}>
            <Pagination
              count={totalPages}
              page={currentPage}
              onChange={handlePageChange}
              color="primary"
            />
          </Box>
        </>
      )}
    </Paper>
  );
};

export default AuditTrail;
