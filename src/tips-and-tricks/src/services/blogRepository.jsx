import axios from 'axios';

export async function getPosts(
	keyword = '',
	pageSize = 10,
	pageNumber = 1,
	sortColumn = '',
	sortOrder = '',
	published = true,
	unpublished = false,
) {
	const response = await axios.get(
		`https://localhost:7157/api/posts?Keyword=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}&Published=${published}&Unpublished=${unpublished}`,
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
