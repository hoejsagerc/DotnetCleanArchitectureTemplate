import http from 'k6/http';
import { check, sleep} from 'k6';

export const options = {
    vus: 1,
    duration: '1m',

    thresholds: {
        http_req_duration: ['p(99)<15'],
    },
};

const BASE_URL = 'http://localhost:5205/api/v1';

export default () => {
    const guest = http.get(`${BASE_URL}/guest/c06017de-3fb3-44f0-ab11-865231ae3534`).json();
    check(guest, { 'id is not null': (obj) => obj.id !== null });
    sleep(1);
};