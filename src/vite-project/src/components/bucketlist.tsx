import { useEffect, useState } from 'react';
import {
  Box,
  Container,
  Paper,
  Stack,
  Typography,
  Switch,
  FormControlLabel,
  Chip,
  List,
  ListItem,
  ListItemText,
  Pagination
} from '@mui/material';
import { apiService } from '../defaults.ts';
import GetAllBucketLists, { BucketListModel } from '../interfaces/GetAllBucketLists.ts';
import createClient from "openapi-fetch";
import type { paths } from "./my-openapi-3-schema";

export default function BucketList() {
  const [data, setData] = useState<GetAllBucketLists>();
  const [page, setPage] = useState(0);
  
  const [unplannedOnly, setUnplannedOnly] = useState<boolean>(false);
  const [loading, setLoading] = useState<boolean>(false);

  const load = () => {
    
    const client = createClient<paths>({})
    
    setLoading(true);
    apiService
      .getBucketList(unplannedOnly ? true : null)
      .then(setData)
      .catch((e) => console.error(e))
      .finally(() => setLoading(false));
  };
  
  const handlePageChange = async (event: unknown, value: number) => {
    setPage(value - 1);
    
    await apiService.getBucketList(unplannedOnly);
  }

  useEffect(() => {
    load();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [unplannedOnly]);

  const renderItem = (item: BucketListModel) => {
    const planned = item.plannedEventId !== null && item.plannedEventId !== undefined;
    return (
      <ListItem key={item.id} divider disableGutters>
        <ListItemText
          primary={
            <Stack direction="row" spacing={1} alignItems="center">
              <Typography fontWeight={600}>{item.friendlyName}</Typography>
              {planned ? <Chip label="Planned" color="success" size="small" /> : <Chip label="Unplanned" size="small" />}
            </Stack>
          }
          secondary={
            <>
              <Typography variant="body2" color="text.secondary">{item.location}</Typography>
              {item.userDisplayName && (
                <Typography variant="caption" color="text.secondary">Suggested by {item.userDisplayName}</Typography>
              )}
            </>
          }
        />
      </ListItem>
    );
  };

  return (
    <Box sx={{ py: 4 }}>
      <Container maxWidth="md">
        <Stack spacing={2}>
          <Typography variant="h4" component="h1" fontWeight={600}>
            Bucket List
          </Typography>

          <Paper className="page-card" elevation={4} sx={{ p: 2, borderRadius: 2 }}>
            <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2} alignItems={{ sm: 'center' }}>
              <FormControlLabel
                control={<Switch checked={unplannedOnly} onChange={(e) => setUnplannedOnly(e.target.checked)} />}
                label="Show unplanned only"
              />
            </Stack>
          </Paper>

          <Paper className="page-card" elevation={4} sx={{ p: 2, borderRadius: 2 }}>
            {loading ? (
              <Typography color="text.secondary">Loading...</Typography>
            ) : data && data.items && data.items.length > 0 ? (
              <>
                <List disablePadding>
                  {data.items.map(renderItem)}
                </List>

                <Pagination count={data.fullCount} page={page + 1} />
                <Typography variant="caption" color="text.secondary">Total: {data.items.length}</Typography>
              </>
            ) : (
              <Typography color="text.secondary">No items found.</Typography>
            )}
          </Paper>
        </Stack>
      </Container>
    </Box>
  );
}
