
import { Avatar, Chip, Grid, IconButton, Paper, Stack, Tooltip, Typography } from "@mui/material";
import Box from "@mui/system/Box";
import { useEffect, useMemo, useState } from "react";
import { styled } from '@mui/material/styles';
import { CustomerList, CustomerListExtended } from "../../types/customer";
import CustomerTable from "../../sections/apps/customer/CustomerTable";
import { PatternFormat } from "react-number-format";
import EditOutlined from "@ant-design/icons/EditOutlined";
import PlusOutlined from "@ant-design/icons/PlusOutlined";
import EyeOutlined from "@ant-design/icons/EyeOutlined";
import DeleteOutlined from "@ant-design/icons/DeleteOutlined";
import { ImagePath, getImageUrl } from "../../utils/getImageUrl";
import { IndeterminateCheckbox } from "../../components/third-party/react-table";
import { ColumnDef } from "@tanstack/react-table";
import CustomerModal from "../../sections/apps/customer/CustomerModal";
import { Gender } from "../../config";
import {CustomerApiModel, CustomerApiModelExtended, mapCustomerApiModelToCustomerList, mapCustomerApiModelToCustomerListExtended} from "../../types/customerApiModel"
import AlertCustomerDelete from "sections/apps/customer/AlertCustomerDelete";



export default function Candidates() {
    const [open, setOpen] = useState<boolean>(false);
    const [allowedCustomers, setAllowedCustomers] = useState<CustomerListExtended[] | undefined>(undefined);
    const [customerModal, setCustomerModal] = useState<boolean>(false);
    const [selectedCustomer, setSelectedCustomer] = useState<CustomerListExtended | null>(null);
    const [customerDeleteId, setCustomerDeleteId] = useState<any>('');

    const handleClose = () => {
        setOpen(!open);
    };

    const columns = useMemo<ColumnDef<CustomerListExtended>[]>(
        () => [
            {
                id: 'select',
                header: ({ table }) => (
                    <IndeterminateCheckbox
                        {...{
                            checked: table.getIsAllRowsSelected(),
                            indeterminate: table.getIsSomeRowsSelected(),
                            onChange: table.getToggleAllRowsSelectedHandler()
                        }}
                    />
                ),
                cell: ({ row }) => (
                    <IndeterminateCheckbox
                        {...{
                            checked: row.getIsSelected(),
                            disabled: !row.getCanSelect(),
                            indeterminate: row.getIsSomeSelected(),
                            onChange: row.getToggleSelectedHandler()
                        }}
                    />
                )
            },
            {
                header: '#',
                accessorKey: 'id',
                meta: {
                    className: 'cell-center'
                }
            },
            {
                header: 'User Info',
                accessorKey: 'name',
                cell: ({ row, getValue }) => (
                    <Stack direction="row" spacing={1.5} alignItems="center">
                        <Avatar
                            alt="Avatar 1"
                            size="sm"
                            src={getImageUrl(`avatar-${!row.original.avatar ? 1 : row.original.avatar}.png`, ImagePath.USERS)}
                        />
                        <Stack spacing={0}>
                            <Typography variant="subtitle1">{getValue() as string}</Typography>
                            <Typography color="text.secondary">{row.original.email as string}</Typography>
                        </Stack>
                    </Stack>
                )
            },
            {
                header: 'Contact',
                accessorKey: 'contact',
                cell: ({ getValue }) => <PatternFormat displayType="text" format="+1 (###) ###-####" mask="_" defaultValue={getValue() as number} />
            },
            {
                header: 'Age',
                accessorKey: 'age',
                meta: {
                    className: 'cell-right'
                }
            },
            {
                header: 'Country',
                accessorKey: 'country'
            },
            {
                header: 'Status',
                accessorKey: 'status',
                cell: (cell) => {
                    switch (cell.getValue()) {
                        case 3:
                            return <Chip color="error" label="Rejected" size="small" variant="light" />;
                        case 1:
                            return <Chip color="success" label="Verified" size="small" variant="light" />;
                        case 2:
                        default:
                            return <Chip color="info" label="Pending" size="small" variant="light" />;
                    }
                }
            },
            {
                header: 'Actions',
                meta: {
                    className: 'cell-center'
                },
                disableSortBy: true,
                cell: ({ row }) => {
                    const collapseIcon =
                        row.getCanExpand() && row.getIsExpanded() ? <PlusOutlined style={{ transform: 'rotate(45deg)' }} /> : <EyeOutlined />;

                    return (
                        <Stack direction="row" alignItems="center" justifyContent="center" spacing={0}>
                            <Tooltip title="View">
                                <IconButton color={row.getIsExpanded() ? 'error' : 'secondary'} 
                                onClick={row.getToggleExpandedHandler()}>
                                    {collapseIcon}
                                </IconButton>
                            </Tooltip>
                            <Tooltip title="Edit">
                                <IconButton
                                    color="primary"
                                    onClick={(e: MouseEvent<HTMLButtonElement>) => {
                                        e.stopPropagation();
                                        console.log("ORIGINAL",row.original);
                                        setSelectedCustomer(row.original);
                                        setCustomerModal(true);
                                    }}
                                >
                                    <EditOutlined />
                                </IconButton>
                            </Tooltip>
                            <Tooltip title="Delete">
                                <IconButton
                                    color="error"
                                    onClick={(e: MouseEvent<HTMLButtonElement>) => {
                                        e.stopPropagation();
                                        setOpen(true);
                                        setCustomerDeleteId(Number(row.original.id));
                                    }}
                                >
                                    <DeleteOutlined />
                                </IconButton>
                            </Tooltip>
                        </Stack>
                    );
                }
            }
        ],
        []
    );

    useEffect(() => {
        const fetchCustomerData = async () => {
            try {
                const customers = await fetchCustomerList();
                // const s = await fetchCustomerList1();
                const mappedCustomers = customers.map((customer: CustomerApiModelExtended) => mapCustomerApiModelToCustomerListExtended(customer));
                console.log(customers)
                setAllowedCustomers(mappedCustomers);

            } catch (error) {
                console.error('Error fetching data:', error);
            }
        };

        fetchCustomerData();
    }, [customerModal,open]);

    // function setCustomerModal(arg0: boolean) {
    //     throw new Error("Function not implemented.");
    // }

    // function setSelectedCustomer(arg0: null) {
    //     throw new Error("Function not implemented.");
    // }

    return (
        <Grid>
            {allowedCustomers === undefined ? <>Loading...</> : <>
                <CustomerTable 
                    data={allowedCustomers} 
                    columns={columns} modalToggler={() => {
                    setCustomerModal(true);
                    setSelectedCustomer(null);
                    }} />
                <AlertCustomerDelete id={Number(customerDeleteId)} title={customerDeleteId} open={open} handleClose={handleClose} />
                <CustomerModal open={customerModal} modalToggler={setCustomerModal} customer={selectedCustomer} />
            </>
            }
        </Grid>
    )

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