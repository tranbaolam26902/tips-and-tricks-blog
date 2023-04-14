// Libraries
import axios from 'axios';

export async function getDashboardData() {
	const { data } = await axios.get(
		`${process.env.REACT_APP_API_ENDPOINT}/dashboard`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}
