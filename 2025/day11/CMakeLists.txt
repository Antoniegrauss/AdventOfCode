add_executable(day11)
target_sources(day11
    PRIVATE
    day11.cpp)
target_link_libraries(day11 read_file)
target_include_directories(day11 PRIVATE ../utils)
