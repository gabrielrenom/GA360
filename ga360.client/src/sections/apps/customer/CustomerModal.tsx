import { useMemo, useEffect, useState } from 'react';
import { useNavigate } from 'react-router';

// material-ui
import Box from '@mui/material/Box';
import Modal from '@mui/material/Modal';
import Stack from '@mui/material/Stack';

// project-imports
import FormCustomerAdd from './FormCustomerAdd';
import MainCard from 'components/MainCard';
import SimpleBar from 'components/third-party/SimpleBar';
import CircularWithPath from 'components/@extended/progress/CircularWithPath';

// types
import { CustomerList, CustomerListExtended } from 'types/customer';
import { getUserById } from 'api/customer';

interface Props {
  open: boolean;
  modalToggler: (state: boolean) => void;
  customer?: CustomerList | null;
}

// ==============================|| CUSTOMER ADD / EDIT ||============================== //

export default function CustomerModal({ open, modalToggler, customer }: Props) {
  const [customerDetails, setCustomerDetails] = useState<CustomerListExtended | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const navigate = useNavigate();

  const closeModal = () => modalToggler(false);

  useEffect(() => {
    if (customer === null)
    {
      setCustomerDetails(null);
    }
    if (customer?.id) {
      setLoading(true);
      getUserById(customer.id)
        .then((customerData) => {
          setCustomerDetails(customerData);
          setLoading(false);
        })
        .catch((error) => {
          console.error('Failed to fetch customer:', error);
          setLoading(false);
        });
    } else {
      setLoading(false);
    }
  }, [customer]);

  const customerForm = useMemo(
    () => !loading && <FormCustomerAdd customer={customerDetails} closeModal={closeModal} />,
    [customerDetails, loading, closeModal]
  );
  // const customerForm = !loading ? <FormCustomerAdd customer={customerDetails} closeModal={closeModal} /> : null;

  return (
    <>
      {open && (
        <Modal
          open={open}
          onClose={closeModal}
          aria-labelledby="modal-customer-add-label"
          aria-describedby="modal-customer-add-description"
          sx={{
            '& .MuiPaper-root:focus': {
              outline: 'none'
            }
          }}
        >
          <MainCard
            sx={{ width: `calc(100% - 48px)`, minWidth: 340, maxWidth: 880, height: 'auto', maxHeight: 'calc(100vh - 48px)' }}
            modal
            content={false}
          >
            <SimpleBar
              sx={{
                maxHeight: `calc(100vh - 48px)`,
                '& .simplebar-content': {
                  display: 'flex',
                  flexDirection: 'column'
                }
              }}
            >
              {loading ? (
                <Box sx={{ p: 5 }}>
                  <Stack direction="row" justifyContent="center">
                    <CircularWithPath />
                  </Stack>
                </Box>
              ) : (
                customerForm
              )}
            </SimpleBar>
          </MainCard>
        </Modal>
      )}
    </>
  );
}
