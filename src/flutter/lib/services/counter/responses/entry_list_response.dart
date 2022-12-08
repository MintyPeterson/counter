import 'package:counter/services/counter/counter_service.dart';
import 'package:counter/services/counter/responses/entry_list_group_response.dart';

/// Represents a response from the [CounterService] entry list endpoint.
class EntryListResponse {

  final List<EntryListGroupResponse> groups;
  EntryListResponse(this.groups);
}