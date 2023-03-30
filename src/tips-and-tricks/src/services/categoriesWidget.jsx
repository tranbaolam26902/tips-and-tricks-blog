import axios from 'axios';

export async function getCategories() {
	const response = await axios.get(
		'https://localhost:7157/api/categories?PageSize=10&PageNumber=1',
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
