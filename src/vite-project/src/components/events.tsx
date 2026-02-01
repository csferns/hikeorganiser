import { useEffect, useState } from 'react'
import GetAllEvents from "../interfaces/GetAllEvents.ts";
import EventCard from "./eventcard.tsx";
import dayjs, { Dayjs } from 'dayjs';
import { DateTimePicker } from "@mui/x-date-pickers";
import { apiService } from '../defaults.ts';
import {
  Box,
  Container,
  Paper,
  Stack,
  Typography,
  Button,
    Grid
} from '@mui/material';

export default function Events() {

    const today = dayjs(new Date());

    const [events, setEvents] = useState<GetAllEvents | undefined>();
    const [startDate, setStartDate] = useState<Dayjs>(today);
    const [endDate, setEndDate] = useState<Dayjs>(today.add(7, 'days'));
    
    const load = () => {
        apiService.getEvents(startDate, endDate)
            .then(setEvents)
            .catch(e => console.error(e));
    };

    useEffect(() => {
        load();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);
    
    return (
        <Box sx={{ py: 4 }}>
            <Container maxWidth="md">
                <Stack spacing={2}>
                    <Typography variant="h4" component="h1" fontWeight={600}>
                        
                        
                        <Grid container>
                            <Grid size={8}>
                                Events
                            </Grid>
                            
                            <Grid size={4} sx={{ display: 'flex', justifyContent: 'flex-end' }}>
                                <Button variant="contained" color="success" href="/schedule">Schedule</Button>
                            </Grid>
                            
                            
                        </Grid>
                    </Typography>

                    <Paper className="page-card" elevation={4} sx={{ p: 2, borderRadius: 2 }}>
                        <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2} alignItems={{ sm: 'center' }}>
                            <DateTimePicker label="Start date" value={startDate} onChange={(d) => d && setStartDate(d)} sx={{ flex: 1 }} />
                            <DateTimePicker label="End date" value={endDate} onChange={(d) => d && setEndDate(d)} sx={{ flex: 1 }} />
                            <Button variant="contained" onClick={load} sx={{ alignSelf: { xs: 'stretch', sm: 'auto' } }}>
                                Load
                            </Button>
                        </Stack>
                    </Paper>

                    <Paper className="page-card" elevation={4} sx={{ p: 2, borderRadius: 2 }}>
                        {events && events.items && events.items.length > 0 ? (
                            <Stack spacing={2}>
                                {events.items.map(e => (
                                    <EventCard event={e} key={e.id} />
                                ))}
                            </Stack>
                        ) : (
                            <Typography color="text.secondary">No events for the selected date range.</Typography>
                        )}
                    </Paper>
                </Stack>
            </Container>
        </Box>
    )
}